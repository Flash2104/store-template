using Store.Service.Contracts.Models;

namespace StoreApi.Models.Auth.SignUp;

public class SignUpResponseDto
{
    public SignUpResponseDto(TokenResponseDto tokenData, UserDto user, ProfileDto profile)
    {
        TokenData = tokenData;
        User = user;
        Profile = profile;
    }

    public TokenResponseDto TokenData { get; }

    public UserDto User { get; }

    public ProfileDto Profile { get; }
}