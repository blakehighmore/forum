using backend.DTOs.PostReactions;


namespace backend.Services.PostReactions;

public interface IPostReactionService
{
    Task<PostReactionReadDto?> ReactAsync(PostReactionDto dto, int currentUserId);
}