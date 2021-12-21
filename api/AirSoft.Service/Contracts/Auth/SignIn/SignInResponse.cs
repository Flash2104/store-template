using Store.Service.Contracts.Jwt.Model;
using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.Auth.SignIn;

public class SignInResponse
{
    public SignInResponse(JwtResponse tokenData, UserData user, AuthProfileData profile)
    {
        TokenData = tokenData;
        User = user;
        Profile = profile;
    }

    public JwtResponse TokenData { get; }

    public UserData User { get; }

    public AuthProfileData Profile { get; }
}