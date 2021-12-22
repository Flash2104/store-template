using Store.Data.Entity;
using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.User.Register;

public class RegisterUserResponse
{
    public RegisterUserResponse(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}