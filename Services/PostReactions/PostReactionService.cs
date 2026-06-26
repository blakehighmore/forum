using backend.Data;
using backend.DTOs.PostReactions;
using backend.Exceptions;
using backend.Models.PostReactions;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.PostReactions;

public class PostReactionService : IPostReactionService
{
    private readonly AppDbContext _db;

    public PostReactionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PostReactionReadDto?> ReactAsync(PostReactionDto dto, int currentUserId)
    {
        var postExisting = await _db.Posts.AnyAsync(p => p.Id == dto.PostId);

        if (!postExisting) throw new NotFoundException("Пост для реакции не был найден");

        var existing = await _db.PostReactions
            .FirstOrDefaultAsync(r => r.UserId == currentUserId && r.PostId == dto.PostId);
        Reaction? resultReaction;

        if (existing is null)
        {
            _db.PostReactions.Add(new PostReaction
            { UserId = currentUserId, PostId = dto.PostId, Reaction = dto.Reaction });
            resultReaction = dto.Reaction;
        }
        else if (existing.Reaction == dto.Reaction)
        {
            _db.PostReactions.Remove(existing);

            resultReaction = null;
        }
        else
        {
            existing.Reaction = dto.Reaction;
            resultReaction = dto.Reaction;
        }

        await _db.SaveChangesAsync();

        return new PostReactionReadDto(dto.PostId, resultReaction);
    }
}