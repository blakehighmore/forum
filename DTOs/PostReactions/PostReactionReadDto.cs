using backend.Models.PostReactions;


namespace backend.DTOs.PostReactions;

public record PostReactionReadDto(int PostId, Reaction? Reaction);