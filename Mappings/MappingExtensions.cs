using backend.DTOs.Tags;
using backend.DTOs.Topics;
using backend.Models.Tags;
using backend.Models.Topics;


namespace backend.Mappings;

public static class MappingExtensions
{
    public static TagReadDto ToReadDto(this Tag tag) => new(tag.Id, tag.Title, tag.Color.Value);

    public static TopicReadDto ToReadDto(this Topic t) => new(t.Id, t.Title, t.Description, t.IsClosed, t.CategoryId,
        t.AuthorId, t.Tags.Select(tag => tag.ToReadDto()).ToList());
}