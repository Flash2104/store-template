using Microsoft.Extensions.Logging;
using Store.Data.Entity;
using Store.Data.InitialData;
using Store.Service.Common;
using Store.Service.Contracts;
using Store.Service.Contracts.Models;
using Store.Service.Contracts.User;
using Store.Service.Contracts.User.Get;
using Store.Service.Contracts.User.Register;
using Store.Service.Exceptions;

namespace Store.Service.Implementations.User;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public UserService(ILogger<UserService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetUserResponse> GetUserByEmailOrPhone(string emailOrPhone)
    {
        emailOrPhone = emailOrPhone.Trim();
        var logPath = $"{emailOrPhone} {nameof(UserService)} {nameof(GetUserByEmailOrPhone)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.EmptyLoginOrPass, "Пустой телефон или почта");
        }
        var isEmail = EmailHelper.IsValidEmail(emailOrPhone);
        DbUser? dbUser = isEmail
            ? await _dataService.Users.GetByEmailAsync(emailOrPhone)
            : await _dataService.Users.GetByPhoneAsync(PhoneHelper.CleanPhone(emailOrPhone));

        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.UserNotFound, "Пользователь не найден");
        }

        return new GetUserResponse(
            new UserData(
                dbUser.Id,
                dbUser.Email,
                dbUser.Phone,
                dbUser.Status,
                dbUser.UserRoles?.Select(x => new ReferenceData<int>(x.Id, x.Role)).ToList()
            ));
    }

    public async Task<bool> ValidateUserPass(int userId, string password)
    {
        var logPath = $"{userId} {nameof(UserService)} {nameof(ValidateUserPass)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.EmptyPassword, "Пустой пароль", logPath);
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);

        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.UserNotFound, "Пользователь не найден", logPath);
        }

        return dbUser.ValidPassword(password);
    }

    public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.EmptyLoginOrPass, "Пустой телефон или почта");
        }
        var logPath = $"{request.PhoneOrEmail} {nameof(UserService)} {nameof(RegisterUser)}. | ";

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.EmptyPassword, "Пустой пароль");
        }
        if (!request.Password.Equals(request.ConfirmPassword))
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.PasswordsNotEqual, "Пароли не совпадают");
        }
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        var isEmail = EmailHelper.IsValidEmail(emailOrPhone);
        DbUser? dbUser = isEmail
            ? await _dataService.Users.GetByEmailAsync(emailOrPhone)
            : await _dataService.Users.GetByPhoneAsync(PhoneHelper.CleanPhone(emailOrPhone));

        if (dbUser != null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.AlreadyExist, "Пользователь с таким телефоном или почтой уже существует");
        }

        var dbUserRole = await _dataService.UserRoles!.GetAsync(x => x.Id == (int)UserRoleType.Customer);
        if (dbUserRole == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.UserRoleNotFound, "Не найдена пользовательская роль");
        }
        
        dbUser = new DbUser()
        {
            Email = isEmail ? emailOrPhone : null,
            Phone = !isEmail ? PhoneHelper.CleanPhone(emailOrPhone) : null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Status = UserStatus.New
        };
        dbUser.PasswordHash = dbUser.HashPassword(request.Password);
        dbUser.UsersToRoles = new List<DbUsersToRoles>()
        {
            new()
            {
                User = dbUser,
                Role = dbUserRole
            }
        };
        var created = this._dataService.Users.CreateNewUser(dbUser);
        _logger.Log(LogLevel.Information, $"{logPath} User created: {created!.Id}.");
        await _dataService.SaveAsync();
        return new RegisterUserResponse(new UserData(
            created.Id,
            created.Email,
            created.Phone,
            created.Status,
            created.UserRoles?.Select(x => new ReferenceData<int>(x.Id, x.Role)).ToList()
            ));
    }
}