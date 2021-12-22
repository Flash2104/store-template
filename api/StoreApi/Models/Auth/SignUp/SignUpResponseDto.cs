using Store.Service.Contracts.Models;

namespace StoreApi.Models.Auth.SignUp;

public class SignUpResponseDto
{
    public SignUpResponseDto(TokenResponseDto tokenData, UserDto user)
    {
        TokenData = tokenData;
        User = user;
    }

    public TokenResponseDto TokenData { get; }

    public UserDto User { get; }
}