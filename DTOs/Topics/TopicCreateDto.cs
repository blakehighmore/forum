namespace backend.DTOs.Topics;

public record TopicCreateDto(string Title, string Description, int CategoryId, List<int>? TagIds);