using System.Security.Claims;
using backend.DTOs;
using backend.DTOs.Posts;
using backend.Models.Users;
using backend.Services.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _service;

    public PostsController(IPostService service)
    {
        _service = service;
    }

    [HttpGet("by-topic/{topicId:int}")]
    public async Task<ActionResult<IEnumerable<PostReadDto>>> GetByTopic(int topicId)
    {
        var posts = await _service.GetByTopicRawAsync(topicId);

        return Ok(posts);
    }

    [HttpGet("search-raw")]
    public async Task<ActionResult<PagedResult<PostReadDto>>> SearchRaw([FromQuery] QueryParameters query)
    {
        var posts = await _service.SearchRawAsync(query);

        return Ok(posts);
    }

    [HttpGet("search-fuzzy")]
    public async Task<ActionResult<IEnumerable<PostReadDto>>> SearchFuzzy([FromQuery] string term)
    {
        var posts = await _service.SearchFuzzyAsync(term);

        return Ok(posts);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<PostReadDto>>> GetAll([FromQuery] QueryParameters queries)
    {
        var posts = await _service.GetAllAsync(queries);

        return Ok(posts);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostReadDto>> GetById(int id)
    {
        var post = await _service.GetByIdAsync(id);

        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<PostReadDto>> Create(PostCreateDto dto)
    {
        var authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var post = await _service.CreateAsync(dto, authorId);

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostReadDto>> Update(int id, PostUpdateDto dto)
    {
        var authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        bool isPrivileged = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.Moderator));


        var updated = await _service.UpdateAsync(id, dto, authorId, isPrivileged);

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var authorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        bool isPrivileged = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.Moderator));


        await _service.DeleteAsync(id, authorId, isPrivileged);

        return NoContent();
    }
}