using backend.Models.PostReactions;


namespace backend.DTOs.PostReactions;

public record PostReactionDto(Reaction Reaction, int PostId);