
using Store.Data.Entity;
using Store.Service.Contracts.Member.Get;
using Microsoft.Extensions.Logging;
using Store.Service.Common;
using Store.Service.Contracts;
using Store.Service.Contracts.Auth;
using Store.Service.Contracts.Auth.SignIn;
using Store.Service.Contracts.Auth.SignUp;
using Store.Service.Contracts.Jwt;
using Store.Service.Contracts.Jwt.Model;
using Store.Service.Contracts.Member;
using Store.Service.Contracts.Member.Create;
using Store.Service.Contracts.Models;
using Store.Service.Contracts.User;
using Store.Service.Contracts.User.Register;
using Store.Service.Exceptions;

namespace Store.Service.Implementations.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly IDataService _dataService;
    private readonly IMemberService _memberService;

    public AuthService(
        ILogger<AuthService> logger,
        IJwtService jwtService,
        IUserService userService,
        IDataService dataService,
        IMemberService memberService
    )
    {
        _logger = logger;
        _jwtService = jwtService;
        _userService = userService;
        _dataService = dataService;
        _memberService = memberService;
    }

    public async Task<SignInResponse> SignIn(SignInRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        var getUser = await _userService.GetUserByEmailOrPhone(emailOrPhone);
        var user = getUser.User;
        var validPassword = await _userService.ValidateUserPass(getUser.User.Id, request.Password);
        if (validPassword)
        {
            _logger.Log(LogLevel.Trace, $"{logPath}. Password valid.");
            MemberData? member = (await _memberService.GetByUserId(user.Id)).MemberData ?? (await _memberService.Create(new CreateMemberRequest(user.Id))).MemberData;
            var tokenData = await _jwtService.BuildToken(new JwtRequest(getUser.User!));
            return new SignInResponse(
                    tokenData,
                    new UserData(
                    user!.Id,
                    user.Email,
                    user.Phone,
                    user.Status,
                    user.UserRoles
                    ),
                    new AuthProfileData(member?.AvatarIcon)
                );
        }

        throw new AirSoftBaseException(ErrorCodes.AuthService.WrongLoginOrPass, "Неверный логин или пароль", logPath);
    }

    public async Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        var emailOrPhone = request.PhoneOrEmail.Trim();
        var logPath = $"{emailOrPhone} {nameof(AuthService)} {nameof(SignIn)}. | ";
        if (string.IsNullOrWhiteSpace(emailOrPhone))
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.EmptyLoginOrPass, "Пустой телефон или почта", logPath);
        }

        var created = await _userService.RegisterUser(new RegisterUserRequest(request.PhoneOrEmail, request.Password,
                request.ConfirmPassword));
        var userData = created.User;
        var member = await _memberService.Create(new CreateMemberRequest(userData.Id));

        var tokenData = await _jwtService.BuildToken(new JwtRequest(userData));

        return new SignUpResponse(
            tokenData,
            new UserData(
                userData!.Id,
                userData.Email,
                userData.Phone,
                userData.Status,
                created.User.UserRoles
                ),
            new AuthProfileData(member?.MemberData?.AvatarIcon)
            );
    }
}
