using System.Security.Claims;
using backend.DTOs;
using backend.DTOs.Topics;
using backend.Models.Users;
using backend.Services.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ITopicService _service;

    public TopicsController(ITopicService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<TopicReadDto>>> GetAll([FromQuery] QueryParameters query)
    {
        var topics = await _service.GetAllAsync(query);

        return Ok(topics);
    }
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TopicReadDto>> GetById(int id)
    {
        var topic = await _service.GetByIdAsync(id);

        return Ok(topic);
    }

    [HttpPost]
    public async Task<ActionResult<TopicReadDto>> Create(TopicCreateDto dto)
    {
        var authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var topic = await _service.CreateAsync(dto, authorId);

        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TopicReadDto>> Update(int id, TopicUpdateDto dto)
    {
        bool isPrivileged = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.Moderator));
        int authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var updated = await _service.UpdateAsync(id, dto, authorId, isPrivileged);

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool isPrivileged = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.Moderator));
        var authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.DeleteAsync(id, authorId, isPrivileged);

        return NoContent();
    }
}