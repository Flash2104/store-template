
using Store.Data.Entity;
using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.Jwt.Model;

public class JwtRequest
{
    public JwtRequest(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}

