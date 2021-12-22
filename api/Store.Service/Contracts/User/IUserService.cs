using Store.Service.Contracts.User.Get;
using Store.Service.Contracts.User.Register;

namespace Store.Service.Contracts.User;

public interface IUserService
{
    Task<GetUserResponse> GetUserByEmailOrPhone(string emailOrPhone);

    Task<bool> ValidateUserPass(Guid userId, string password);

    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);

    Task SetUserTeamManager(Guid userId);
}