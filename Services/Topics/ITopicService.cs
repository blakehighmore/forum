using backend.DTOs;
using backend.DTOs.Topics;


namespace backend.Services.Topics;

public interface ITopicService
{
    Task<PagedResult<TopicReadDto>> GetAllAsync(QueryParameters query);
    Task<TopicReadDto> GetByIdAsync(int id);
    Task<TopicReadDto> CreateAsync(TopicCreateDto dto, int authorId);
    Task<TopicReadDto> UpdateAsync(int id, TopicUpdateDto dto, int authorId, bool isPrivileged);
    Task DeleteAsync(int id, int authorId, bool isPrivileged);
}