using backend.DTOs.Tags;


namespace backend.DTOs.Topics;

public record TopicReadDto(
    int Id,
    string Title,
    string Description,
    bool IsClosed,
    int CategoryId,
    int AuthorId,
    IEnumerable<TagReadDto> Tags);