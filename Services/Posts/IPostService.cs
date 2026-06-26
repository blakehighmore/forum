using backend.DTOs;
using backend.DTOs.Posts;


namespace backend.Services.Posts;

public interface IPostService
{
    Task<IEnumerable<PostReadDto>> GetByTopicRawAsync(int topicId);
    Task<PagedResult<PostReadDto>> SearchRawAsync(QueryParameters query);
    Task<IEnumerable<PostReadDto>> SearchFuzzyAsync(string term);
    Task<PagedResult<PostReadDto>> GetAllAsync(QueryParameters query);
    Task<PostReadDto> GetByIdAsync(int id);
    Task<PostReadDto> CreateAsync(PostCreateDto dto, int authorId);
    Task<PostReadDto> UpdateAsync(int id, PostUpdateDto dto, int authorId, bool isPrivileged);
    Task DeleteAsync(int id, int authorId, bool isPrivileged);
}