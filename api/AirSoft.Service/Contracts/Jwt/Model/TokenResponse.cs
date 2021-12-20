
namespace AirSoft.Service.Contracts.Jwt.Model
{
    public class JwtResponse
    {
        public JwtResponse(string? token, DateTime? expiryDate)
        {
            Token = token;
            ExpiryDate = expiryDate;
        }

        public string? Token { get; }

        public DateTime? ExpiryDate { get; }
    }
}
