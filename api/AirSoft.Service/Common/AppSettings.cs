
namespace AirSoft.Service.Common
{
    public class AppSettings
    {
        public JwtSettings? Jwt { get; set; }
    }

    public class JwtSettings
    {
        public string? Key { get; set; }

        public string? Issuer { get; set; }

        public int? ExpiresSeconds { get; set; }
    }
}
