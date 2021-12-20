namespace AirSoftApi.Models.Auth.SignIn;

public class SignInResponseDto
{
    public SignInResponseDto(TokenResponseDto tokenData, UserDto user, ProfileDto profile)
    {
        TokenData = tokenData;
        User = user;
        Profile = profile;
    }

    public TokenResponseDto TokenData { get; }

    public UserDto User { get; }

    public ProfileDto Profile { get; }
}