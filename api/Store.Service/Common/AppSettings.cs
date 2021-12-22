
namespace Store.Service.Common
{
    public class AppSettings
    {
        public JwtSettings? Jwt { get; set; }

        public CacheSettings? Cache { get; set; }
    }

    public class JwtSettings
    {
        public string? Key { get; set; }

        public string? Issuer { get; set; }

        public int? ExpiresSeconds { get; set; }
    }

    public class CacheSettings
    {

    }
}
