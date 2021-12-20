namespace AirSoftApi.Models.Auth;

public class TokenResponseDto
{
    public TokenResponseDto(string? token, DateTime? expiryDate)
    {
        Token = token;
        ExpiryDate = expiryDate;
    }

    public string? Token { get; }

    public DateTime? ExpiryDate { get; }
}