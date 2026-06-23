namespace backend.DTOs;

public record PagedResult<T>(IEnumerable<T> items, int Page, int PageSize, int totalCount)
{
    public int TotalPage => (int)Math.Ceiling((double)totalCount / PageSize);
}