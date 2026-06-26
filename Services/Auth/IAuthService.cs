using backend.DTOs.Auth;


namespace backend.Services.Auth;

public interface IAuthService
{

    Task<LoginReadDto> RegisterAsync(RegisterDto dto);
    Task<TokenPairDto> LoginAsync(LoginDto dto);
    Task<TokenPairDto> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}