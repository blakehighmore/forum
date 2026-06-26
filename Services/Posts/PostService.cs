using backend.Data;
using backend.DTOs;
using backend.DTOs.Posts;
using backend.Exceptions;
using backend.Models.Posts;
using backend.Utils;
using Dapper;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Posts;

public class PostService : IPostService
{
    private readonly AppDbContext _db;
    private readonly IDbConnectionFactory _connectionFactory;

    public PostService(AppDbContext db, IDbConnectionFactory connectionFactory)
    {
        _db = db;
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<PostReadDto>> GetByTopicRawAsync(int topicId)
    {
        const string sql = """
                           SELECT "Id", "Content", "CreatedAt", "UpdatedAt", "AuthorId", "TopicId"
                           FROM "Posts"
                           WHERE "TopicId" = @topicId
                           ORDER BY "CreatedAt" DESC 
                           """;
        using var connection = _connectionFactory.Create();

        return await connection.QueryAsync<PostReadDto>(sql, new { topicId });
    }

    public async Task<PagedResult<PostReadDto>> SearchRawAsync(QueryParameters query)
    {
        var pagination = Pagination.From(query.Page, query.PageSize);
        var search = string.IsNullOrWhiteSpace(query.Search) ? null : $"%{query.Search}%";

        const string dataSql = """
                               SELECT "Id", "Content", "CreatedAt", "UpdatedAt", "AuthorId", "TopicId"
                               FROM "Posts"
                               WHERE (@search IS NULL OR "Content" ILIKE @search)
                               ORDER BY "CreatedAt" DESC
                               LIMIT @take OFFSET @skip
                               """;

        const string countSql = """
                                SELECT COUNT(*)
                                FROM "Posts"
                                WHERE (@search IS NULL OR "Content" ILIKE @search)

                                """;
        using var connection = _connectionFactory.Create();

        var items = await connection.QueryAsync<PostReadDto>(dataSql,
            new { search, take = pagination.PageSize, skip = pagination.Skip });
        var total = await connection.ExecuteScalarAsync<int>(countSql, new { search });

        return new PagedResult<PostReadDto>(items, pagination.Page, pagination.PageSize, total);
    }

    public async Task<IEnumerable<PostReadDto>> SearchFuzzyAsync(string term)
    {
        const string sql = """
                           
                           SELECT "Id", "Content", "CreatedAt", "UpdatedAt", "AuthorId", "TopicId"
                           FROM "Posts"
                           WHERE @term <% "Content"
                           ORDER BY word_similarity(@term, "Content") DESC
                           LIMIT 10
                           """;
        using var connection = _connectionFactory.Create();

        return await connection.QueryAsync<PostReadDto>(sql, new { term });
    }

    public async Task<PagedResult<PostReadDto>> GetAllAsync(QueryParameters query)
    {
        var pagination = Pagination.From(query.Page, query.PageSize);
        var postQueries = _db.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            postQueries = postQueries.Where(p => EF.Functions.ILike(p.Content, $"%{query.Search}%"));

        var totalCount = await postQueries.CountAsync();

        postQueries = query.SortBy?.ToLower() switch
        {
            "content" => query.Desc
                ? postQueries.OrderByDescending(p => p.Content)
                : postQueries.OrderBy(p => p.Content),
            _ => postQueries.OrderBy(p => p.Id)
        };

        var items = await postQueries.Skip(pagination.Skip).Take(pagination.PageSize).Select(p =>
            new PostReadDto(p.Id, p.Content, p.CreatedAt, p.UpdatedAt, p.AuthorId, p.TopicId)).ToListAsync();

        return new PagedResult<PostReadDto>(items, pagination.Page, pagination.PageSize, totalCount);
    }

    public async Task<PostReadDto> GetByIdAsync(int id)
    {
        var post = await _db.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostReadDto(p.Id, p.Content, p.CreatedAt, p.UpdatedAt, p.AuthorId, p.TopicId))
            .FirstOrDefaultAsync();

        if (post is null) throw new NotFoundException("Пост не был найден");

        return post;
    }

    public async Task<PostReadDto> CreateAsync(PostCreateDto dto, int authorId)
    {
        if (!await _db.Topics.AnyAsync(t => t.Id == dto.TopicId))
            throw new NotFoundException("Тема для поста не была найдена");

        var post = new Post()
        {
            AuthorId = authorId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            TopicId = dto.TopicId
        };
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        return new PostReadDto(post.Id, post.Content, post.CreatedAt, post.UpdatedAt, post.AuthorId, post.TopicId);
    }

    public async Task<PostReadDto> UpdateAsync(int id, PostUpdateDto dto, int authorId, bool isPrivileged)
    {
        var post = await _db.Posts.FindAsync(id);

        if (post is null) throw new NotFoundException("Пост не был найден");

        if (post.AuthorId != authorId && !isPrivileged)
            throw new ForbiddenException("Редактировать можно только свой контент");

        post.Content = dto.Content ?? post.Content;
        post.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new PostReadDto(post.Id, post.Content, post.CreatedAt, post.UpdatedAt, post.AuthorId, post.TopicId);
    }

    public async Task DeleteAsync(int id, int authorId, bool isPrivileged)
    {
        var post = await _db.Posts.FindAsync(id);

        if (post is null) throw new NotFoundException("Пост не был найден");

        if (post.AuthorId != authorId && !isPrivileged)
            throw new ForbiddenException("Редактировать можно только свой контент");
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
    }
}