using Store.Service.Contracts.Jwt.Model;
using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.Auth.SignUp;

public class SignUpResponse
{
    public SignUpResponse(JwtResponse tokenData, UserData user)
    {
        TokenData = tokenData;
        User = user;
    }

    public JwtResponse TokenData { get; }
    public UserData User { get; }
}