using backend.Data;
using backend.DTOs;
using backend.DTOs.Topics;
using backend.Exceptions;
using backend.Mappings;
using backend.Models.Topics;
using backend.Utils;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Topics;

public class TopicService : ITopicService
{
    private readonly AppDbContext _db;

    public TopicService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<TopicReadDto>> GetAllAsync(QueryParameters query)
    {
        var topicQuery = _db.Topics.AsQueryable();
        var p = Pagination.From(query.Page, query.PageSize);

        if (!string.IsNullOrWhiteSpace(query.Search))
            topicQuery = topicQuery.Where(t =>
                EF.Functions.ILike(t.Title, $"%{query.Search}%") ||
                EF.Functions.ILike(t.Description, $"%{query.Search}%"));

        var totalCount = await topicQuery.CountAsync();

        topicQuery = query.SortBy?.ToLower() switch
        {
            "title" => query.Desc ? topicQuery.OrderByDescending(t => t.Title) : topicQuery.OrderBy(t => t.Title),
            "description" => query.Desc
                ? topicQuery.OrderByDescending(t => t.Description)
                : topicQuery.OrderBy(t => t.Description),
            _ => topicQuery.OrderBy(t => t.Id)
        };
        var topics = await topicQuery.Skip(p.Skip).Take(p.PageSize).Include(t => t.Tags).ToListAsync();

        var items = topics.Select(t => t.ToReadDto()).ToList();

        return new PagedResult<TopicReadDto>(items, p.Page, p.PageSize, totalCount);
    }


    public async Task<TopicReadDto> GetByIdAsync(int id)
    {
        var topic = await _db.Topics.AsNoTracking().Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id);

        if (topic is null) throw new NotFoundException("Тема не была найдена");


        return topic.ToReadDto();
    }

    public async Task<TopicReadDto> CreateAsync(TopicCreateDto dto, int authorId)
    {
        var isExist = await _db.Topics.AnyAsync(t => t.Title == dto.Title);
        var isCategoryExist = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);

        if (isExist) throw new ConflictException("Тема с таким названием уже существует");

        if (!isCategoryExist) throw new NotFoundException("Категория не была найдена");
        var requestedTags = dto.TagIds ?? [];
        var tags = await _db.Tags.Where(tag => requestedTags.Contains(tag.Id)).ToListAsync();

        if (tags.Count != requestedTags.Distinct().Count())
            throw new NotFoundException("Один или несколько тегов не найдены");

        Topic topic = new()
        {
            Title = dto.Title,
            Description = dto.Description,
            AuthorId = authorId,
            CategoryId = dto.CategoryId
        };
        foreach (var tag in tags) topic.AddTag(tag);

        _db.Topics.Add(topic);
        await _db.SaveChangesAsync();

        return topic.ToReadDto();
    }

    public async Task<TopicReadDto> UpdateAsync(int id, TopicUpdateDto dto, int authorId, bool isPrivileged)
    {
        var topic = await _db.Topics.Include(t => t.Tags).Where(t => t.Id == id).FirstOrDefaultAsync();

        if (topic is null) throw new NotFoundException("Тема не была найдена");

        if (topic.AuthorId != authorId && !isPrivileged)
            throw new ForbiddenException("Редактировать можно только свой контент");

        topic.Title = dto.Title ?? topic.Title;
        topic.Description = dto.Description ?? topic.Description;

        await _db.SaveChangesAsync();

        return topic.ToReadDto();
    }

    public async Task DeleteAsync(int id, int authorId, bool isPrivileged)
    {
        var topic = await _db.Topics.FindAsync(id);

        if (topic is null) throw new NotFoundException("Тема не была найдена");

        if (topic.AuthorId != authorId && !isPrivileged)
            throw new ForbiddenException("Редактировать можно только свой контент");

        _db.Topics.Remove(topic);
        await _db.SaveChangesAsync();
    }
}