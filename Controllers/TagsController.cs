using System.Security.Claims;
using backend.DTOs.Tags;
using backend.Models.Users;
using backend.Services.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _service;

    public TagsController(ITagService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagReadDto>>> GetAll()
    {
        var tags = await _service.GetAllAsync();

        return Ok(tags);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagReadDto>> GetById(int id)
    {
        var tag = await _service.GetByIdAsync(id);

        return Ok(tag);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<TagReadDto>> Create(TagCreateDto dto)
    {
        var tag = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TagReadDto>> Update(int id, TagUpdateDto dto)
    {
        var tag = await _service.UpdateAsync(id, dto);

        return Ok(tag);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}