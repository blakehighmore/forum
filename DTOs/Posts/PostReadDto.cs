namespace backend.DTOs.Posts;

public record PostReadDto(int Id, string Content, DateTime CreatedAt, DateTime? UpdatedAt, int AuthorId, int TopicId);