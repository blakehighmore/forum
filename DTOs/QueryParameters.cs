namespace backend.DTOs;

public record QueryParameters(string? Search, string? SortBy, bool Desc, int? Page, int? PageSize);