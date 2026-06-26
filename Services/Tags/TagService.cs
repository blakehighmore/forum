using backend.Data;
using backend.DTOs.Tags;
using backend.Exceptions;
using backend.Mappings;
using backend.Models.Tags;
using backend.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Tags;

public class TagService : ITagService
{
    private readonly AppDbContext _db;

    public TagService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TagReadDto>> GetAllAsync()
    {
        var tags = await _db.Tags.ToListAsync();

        return tags.Select(t => new TagReadDto(t.Id, t.Title, t.Color.Value));
    }

    public async Task<TagReadDto> GetByIdAsync(int id)
    {
        var tag = await _db.Tags
            .Where(t => t.Id == id)
            .Select(t => new TagReadDto(t.Id, t.Title, t.Color.Value))
            .FirstOrDefaultAsync();

        if (tag is null) throw new NotFoundException("Тег не был найден");

        return tag;
    }

    public async Task<TagReadDto> CreateAsync(TagCreateDto dto)
    {
        var isExist = await _db.Tags.AnyAsync(t => t.Title == dto.Title);

        if (isExist) throw new ConflictException("Тег с таким названием уже существует");

        var tag = new Tag()
        {
            Title = dto.Title,
            Color = Color.Create(dto.Color)
        };
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();

        return tag.ToReadDto();
    }

    public async Task<TagReadDto> UpdateAsync(int id, TagUpdateDto dto)
    {
        var tag = await _db.Tags.FindAsync(id);

        if (tag is null) throw new NotFoundException("Тег не был найден");
        tag.Title = dto.Title ?? tag.Title;
        tag.Color = Color.Create(dto.Color ?? tag.Color.Value);
        await _db.SaveChangesAsync();

        return tag.ToReadDto();
    }

    public async Task DeleteAsync(int id)
    {
        var tag = await _db.Tags.FindAsync(id);

        if (tag is null) throw new NotFoundException("Тег не был найден");
        _db.Tags.Remove(tag);
        await _db.SaveChangesAsync();
    }
}