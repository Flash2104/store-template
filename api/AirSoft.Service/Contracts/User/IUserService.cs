using AirSoft.Service.Contracts.User.Get;
using AirSoft.Service.Contracts.User.Register;

namespace AirSoft.Service.Contracts.User;

public interface IUserService
{
    Task<GetUserResponse> GetUserByEmailOrPhone(string emailOrPhone);

    Task<bool> ValidateUserPass(Guid userId, string password);

    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);

    Task SetUserTeamManager(Guid userId);
}