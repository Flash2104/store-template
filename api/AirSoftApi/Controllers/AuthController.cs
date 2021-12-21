using Store.Service.Contracts.Auth;
using Store.Service.Contracts.Auth.SignIn;
using Store.Service.Contracts.Auth.SignUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Models.Auth;
using StoreApi.Models.Auth.SignIn;
using StoreApi.Models.Auth.SignUp;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : RootController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<ServerResponseDto<SignInResponseDto>> SignIn([FromBody] SignInRequestDto request)
        {
            var logPath = $"{request.PhoneOrEmail}.{nameof(AuthController)} {nameof(SignIn)} | ";
            return await HandleRequest(
                _authService.SignIn,
                request,
                dto => new SignInRequest(dto.PhoneOrEmail, dto.Password),
                res => new SignInResponseDto(
                    new TokenResponseDto(res.TokenData.Token, res.TokenData.ExpiryDate),
                    new UserDto(res.User.Id, res.User.Email, res.User.Phone),
                    new ProfileDto(res.Profile.AvatarIcon)
                ),
                logPath
            );
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<ServerResponseDto<SignUpResponseDto>> SignUp([FromBody] SignUpRequestDto request)
        {
            var logPath = $"{request.PhoneOrEmail}.{nameof(AuthController)} {nameof(SignIn)} | ";
            return await HandleRequest(
                _authService.SignUp,
                request,
                dto => new SignUpRequest(dto.PhoneOrEmail, dto.Password, dto.ConfirmPassword),
                res => new SignUpResponseDto(
                    new TokenResponseDto(res.TokenData.Token, res.TokenData.ExpiryDate),
                    new UserDto(res.User.Id, res.User.Email, res.User.Phone),
                    new ProfileDto(res.Profile.AvatarIcon)
                    ),
                logPath
            );
        }
    }
}
