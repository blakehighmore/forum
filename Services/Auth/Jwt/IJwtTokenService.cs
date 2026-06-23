using backend.Models.Users;


namespace backend.Services.Auth.Jwt;

public interface IJwtTokenService
{
     string JwtTokenGenerate(User user);
}