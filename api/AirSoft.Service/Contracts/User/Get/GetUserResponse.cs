using Store.Service.Contracts.Models;

namespace Store.Service.Contracts.User.Get;

public class GetUserResponse
{
    public GetUserResponse(UserData user)
    {
        User = user;
    }

    public UserData User { get; }
}