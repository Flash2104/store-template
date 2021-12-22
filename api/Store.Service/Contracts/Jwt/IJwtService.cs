using Store.Service.Contracts.Jwt.Model;

namespace Store.Service.Contracts.Jwt;
public interface IJwtService
{
    Task<JwtResponse> BuildToken(JwtRequest request);
}
