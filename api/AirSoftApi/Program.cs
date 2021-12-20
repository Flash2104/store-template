using System.Net;
using AirSoft.Data;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Navigation;
using AirSoft.Service.Contracts.References;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.User;
using AirSoft.Service.Implementations;
using AirSoft.Service.Implementations.Auth;
using AirSoft.Service.Implementations.Jwt;
using AirSoft.Service.Implementations.Member;
using AirSoft.Service.Implementations.References;
using AirSoft.Service.Implementations.Team;
using AirSoft.Service.Implementations.User;
using AirSoftApi.Filters;
using AirSoftApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var parentDir = hostingContext.HostingEnvironment.ContentRootPath;
    var path = string.Concat(parentDir, "\\ConfigSource\\appsettings.json");

    config.AddJsonFile(path);
});
var settingsSection = builder.Configuration.GetSection(nameof(AppSettings));
var appSettings = settingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(settingsSection);
var configService = new ConfigService(appSettings);

builder.Host.UseNLog();
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var userId = context?.Principal?.Identity?.Name;

                // var user = userService.GetById(userId);
                if (string.IsNullOrEmpty(userId))
                {
                    // return unauthorized if user no longer exists
                    context!.Fail("Unauthorized");
                }
                return Task.CompletedTask;
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                var response = new ServerResponseDto(new ErrorDto(403, "Недостаточно прав"));
                await HttpResponseWritingExtensions.WriteAsync(context.Response, JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            },
            OnAuthenticationFailed = async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                var response = new ServerResponseDto(new ErrorDto(401, "Неавторизованный запрос"));
                await HttpResponseWritingExtensions.WriteAsync(context.Response, JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            }
        };
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt?.Key ?? throw new ApplicationException("Jwt settings in null")))
        };
    });


builder.Services.AddDbContext<AirSoftDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirSoftDatabase"), opt => opt.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));
builder.Services.AddScoped<IDbContext, AirSoftDbContext>();
builder.Services.AddScoped<IConfigService, ConfigService>(p => configService);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<ICorrelationService, CorrelationService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IReferenceService, ReferenceService>();

builder.Services.AddControllers();
builder.Services.AddMvc(opt =>
{
    opt.Filters.Add<CorrelationInitializeActionFilter>();
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var context = serviceScope.ServiceProvider.GetService<IDbContext>();
context?.Initialize();

string root = app.Environment.ContentRootPath;
NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(root + "\\ConfigSource\\NLog.config");
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(b =>
{
    b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();