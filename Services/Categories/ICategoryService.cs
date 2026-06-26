using backend.DTOs;
using backend.DTOs.Categories;


namespace backend.Services.Categories;

public interface ICategoryService
{
    Task<PagedResult<CategoryReadDto>> GetAllAsync(QueryParameters query);
    Task<CategoryReadDto> GetByIdAsync(int id);
    Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryReadDto> UpdateAsync(int id, CategoryUpdateDto dto);
    Task DeleteAsync(int id);
}