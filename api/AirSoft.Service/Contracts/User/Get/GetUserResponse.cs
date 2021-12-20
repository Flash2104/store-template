using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.User.Get;

public class GetUserResponse
{
    public GetUserResponse(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}