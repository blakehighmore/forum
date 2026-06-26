using backend.DTOs.Auth;
using backend.Services.Auth;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginReadDto>> Register(RegisterDto dto)
    {
        var registered = await _service.RegisterAsync(dto);

        return Ok(registered);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);


        return Ok(new { token.AccessToken, token.RefreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequestDto dto)
    {
        var tokens = await _service.RefreshAsync(dto.RefreshToken);

        return Ok(tokens);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequestDto dto)
    {
        await _service.LogoutAsync(dto.RefreshToken);

        return NoContent();
    }
}