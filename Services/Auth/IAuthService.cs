using backend.DTOs.Auth;


namespace backend.Services.Auth;

public interface IAuthService
{
    Task<LoginReadDto> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}