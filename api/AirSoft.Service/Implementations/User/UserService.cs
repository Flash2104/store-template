using AirSoft.Data.Entity;
using AirSoft.Data.InitialData;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.User;
using AirSoft.Service.Contracts.User.Get;
using AirSoft.Service.Contracts.User.Register;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.User;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IMemberService _memberService;
    private readonly IDataService _dataService;

    public UserService(ILogger<UserService> logger, ICorrelationService correlationService, IMemberService memberService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _memberService = memberService;
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

    public async Task<bool> ValidateUserPass(Guid userId, string password)
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

        var dbUserRole = await _dataService.UserRoles!.GetAsync(x => x.Id == (int)UserRoleType.Player);
        if (dbUserRole == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.UserRoleNotFound, "Не найдена пользовательская роль");
        }

        var id = Guid.NewGuid();
        dbUser = new DbUser()
        {
            Id = id,
            Email = isEmail ? emailOrPhone : null,
            Phone = !isEmail ? PhoneHelper.CleanPhone(emailOrPhone) : null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = id,
            ModifiedBy = id,
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
        await this.CreateUserNavigation(created.Id);
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

    public async Task SetUserTeamManager(Guid userId)
    {
        var logPath = $"{userId} {nameof(UserService)} {nameof(SetUserTeamManager)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);
        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserService.UserNotFound, "Пользователь не найден", logPath);
        }

        var dbManager = new DbUserRole()
        {
            Id = (int)UserRoleType.TeamManager,
            Role = UserRoleType.TeamManager.ToString()
        };
        await AddTeamManagerNavigation(userId);
        await _dataService.Users.AddUserRole(userId, dbManager);
    }

    private Task<DbUserNavigation> CreateUserNavigation(Guid userId)
    {
        DbUserNavigation dbNavigation = new DbUserNavigation()
        {
            UserId = userId,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Title = "Навигация пользователя",
            Id = Guid.NewGuid(),
            IsDefault = true
        };
        dbNavigation.NavigationsToNavigationItems = RoleNavigationItemsConst.PlayerNavItemIds.Select(y => new DbNavigationsToNavigationItems()
        {
            NavigationId = dbNavigation.Id,
            NavigationItemId = y
        }).ToList();
        _dataService.UserNavigations.Insert(dbNavigation);
        return Task.FromResult(dbNavigation);
    }

    private async Task<DbUserNavigation> AddTeamManagerNavigation(Guid userId)
    {
        var userNavigations = await _dataService.UserNavigations.ListAsync(x => x.UserId == userId);
        foreach (var navigation in userNavigations)
        {
            navigation.IsDefault = false;
            _dataService.UserNavigations.Update(navigation);
        }
        DbUserNavigation dbNewNavigation = new DbUserNavigation()
        {
            UserId = userId,
            CreatedBy = userId,
            ModifiedBy = userId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            Title = "Навигация Менеджера команды",
            IsDefault = true
        };
        var created = _dataService.UserNavigations.Insert(dbNewNavigation);

        dbNewNavigation.NavigationsToNavigationItems = RoleNavigationItemsConst.TeamManagerNavItemIds.Select(y => new DbNavigationsToNavigationItems()
        {
            NavigationId = dbNewNavigation.Id,
            NavigationItemId = y
        }).ToList();
        return dbNewNavigation;
    }
}