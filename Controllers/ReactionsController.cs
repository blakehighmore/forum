using System.Security.Claims;
using backend.DTOs.PostReactions;
using backend.Services.PostReactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReactionsController : ControllerBase
{
    private readonly IPostReactionService _service;

    public ReactionsController(IPostReactionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PostReactionReadDto>> React(PostReactionDto dto)
    {
        int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var reaction = await _service.ReactAsync(dto, currentUserId);

        if (reaction is null) return NoContent();

        return Ok(reaction);
    }
}