
using AirSoft.Data.Entity;
using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Jwt.Model;

public class JwtRequest
{
    public JwtRequest(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}

