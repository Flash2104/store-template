using AirSoft.Data.Entity;
using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.User.Register;

public class RegisterUserResponse
{
    public RegisterUserResponse(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}