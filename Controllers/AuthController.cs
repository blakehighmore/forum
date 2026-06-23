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

        if (token is null) return Unauthorized();

        return Ok(new { token });
    }
}