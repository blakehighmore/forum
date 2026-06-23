using backend.Data;
using backend.DTOs.Auth;
using backend.Exceptions;
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
        if (isExist)  throw new ConflictException("Такое имя пользователя уже существует");

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

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.Where(u => u.Username == dto.Username).FirstOrDefaultAsync();

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) throw new NotFoundException("Неверные данные");

        return _tokenService.JwtTokenGenerate(user);



    }
}