using AirSoft.Service.Contracts.Jwt.Model;

namespace AirSoft.Service.Contracts.Jwt;
public interface IJwtService
{
    Task<JwtResponse> BuildToken(JwtRequest request);
}
