using backend.Data;
using backend.DTOs;
using backend.DTOs.Categories;
using backend.Exceptions;
using backend.Models.Categories;
using backend.Utils;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<CategoryReadDto>> GetAllAsync(QueryParameters query)
    {
        var p = Pagination.From(query.Page, query.PageSize);
        var categoryQuery = _db.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            categoryQuery = categoryQuery.Where(c =>
                EF.Functions.ILike(c.Title, $"%{query.Search}%") || EF.Functions.ILike(c.Description, $"%{query.Search}%")
            );
        int totalCount = await categoryQuery.CountAsync();

        categoryQuery = query.SortBy?.ToLower() switch
        {
            "title" => query.Desc ? categoryQuery.OrderByDescending(c => c.Title) : categoryQuery.OrderBy(c => c.Title),
            "description" => query.Desc
                ? categoryQuery.OrderByDescending(c => c.Description)
                : categoryQuery.OrderBy(c => c.Description),
            _ => categoryQuery.OrderBy(c => c.Id)
        };

        var items = await categoryQuery
            .Skip(p.Skip)
            .Take(p.PageSize)
            .Select(ps => new CategoryReadDto(ps.Id, ps.Title, ps.Description))
            .ToListAsync();



        return new PagedResult<CategoryReadDto>(items, p.Page, p.PageSize, totalCount);
    }

    public async Task<CategoryReadDto> GetByIdAsync(int id)
    {
        var category = await _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) throw new NotFoundException("Категория не найдена");

        return new CategoryReadDto(category.Id, category.Title, category.Description);
    }

    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
    {
        bool isExist = await _db.Categories.AnyAsync(c => c.Title == dto.Title);

        if (isExist) throw new ConflictException("Категория с таким названием уже существует");

        Category category = new()
        {
            Title = dto.Title,
            Description = dto.Description
        };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return new CategoryReadDto(category.Id, category.Title, category.Description);
    }

    public async Task<CategoryReadDto> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null) throw new NotFoundException("Категория не была найдена");
        category.Title = dto.Title ?? category.Title;
        category.Description = dto.Description ?? category.Description;

        await _db.SaveChangesAsync();

        return new CategoryReadDto(category.Id, category.Title, category.Description);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null) throw new NotFoundException("Категория не была найдена");
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}