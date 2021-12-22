namespace StoreApi.Models.Auth.SignIn;

public class SignInResponseDto
{
    public SignInResponseDto(TokenResponseDto tokenData, UserDto user)
    {
        TokenData = tokenData;
        User = user;
    }

    public TokenResponseDto TokenData { get; }

    public UserDto User { get; }
}