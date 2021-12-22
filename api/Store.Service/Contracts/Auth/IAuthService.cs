using Store.Service.Contracts.Auth.SignIn;
using Store.Service.Contracts.Auth.SignUp;

namespace Store.Service.Contracts.Auth;

public interface IAuthService
{
    Task<SignInResponse> SignIn(SignInRequest request);

    Task<SignUpResponse> SignUp(SignUpRequest request);
}