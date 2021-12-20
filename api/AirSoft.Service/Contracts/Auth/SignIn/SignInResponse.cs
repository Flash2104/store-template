using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Auth.SignIn;

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