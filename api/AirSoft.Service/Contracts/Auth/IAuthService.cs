using AirSoft.Service.Contracts.Auth.SignIn;
using AirSoft.Service.Contracts.Auth.SignUp;

namespace AirSoft.Service.Contracts.Auth;

public interface IAuthService
{
    Task<SignInResponse> SignIn(SignInRequest request);

    Task<SignUpResponse> SignUp(SignUpRequest request);
}