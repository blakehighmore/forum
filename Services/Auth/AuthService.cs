using System.Security.Cryptography;
using backend.Data;
using backend.DTOs.Auth;
using backend.Exceptions;
using backend.Models.Auth;
using backend.Models.Profiles;
using backend.Models.Users;
using backend.Services.Auth.Jwt;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IJwtTokenService _tokenService;

    public AuthService(AppDbContext db, IJwtTokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }


    public async Task<LoginReadDto> RegisterAsync(RegisterDto dto)
    {
        var isExist = await _db.Users.AnyAsync(u => u.Username == dto.Username);

        if (isExist) throw new ConflictException("Такое имя пользователя уже существует");

        var user = new User()
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = Role.User,
            Profile = new Profile(),
            CreatedAt = DateTime.UtcNow,
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new LoginReadDto(user.Id, user.Username);
    }

    public async Task<TokenPairDto> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.Where(u => u.Username == dto.Username).FirstOrDefaultAsync();

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException("Неверные данные");
        var accessToken = _tokenService.JwtTokenGenerate(user);

        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return new TokenPairDto(accessToken, refreshToken.Token);
    }

    public async Task<TokenPairDto> RefreshAsync(string refreshToken)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (stored is null || !stored.IsActive) throw new UnauthorizedException("Невалидный refresh-token");
        var user = await _db.Users.FindAsync(stored.UserId);

        if (user is null) throw new NotFoundException("Пользователь не найден");
        stored.RevokedAt = DateTime.UtcNow;
        var newAccess = _tokenService.JwtTokenGenerate(user);

        var newRefresh = new RefreshToken
        {
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        _db.RefreshTokens.Add(newRefresh);
        await _db.SaveChangesAsync();

        return new TokenPairDto(newAccess, newRefresh.Token);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (stored is null || !stored.IsActive) return;
        stored.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(bytes);
    }
}