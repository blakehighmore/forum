using System.Security.Claims;
using backend.DTOs.Profiles;
using backend.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _service;

    public ProfilesController(IProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileReadDto>> Get()
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var profile = await _service.GetAsync(currentUserId);

        return Ok(profile);
    }

    [HttpPut]
    public async Task<ActionResult<ProfileReadDto>> Update(ProfileUpdateDto dto)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _service.UpdateAsync(currentUserId, dto);

        return Ok(updated);
    }
}