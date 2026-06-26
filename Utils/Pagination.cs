namespace backend.Utils;

public readonly record struct Pagination
{
    public const int DefaultSize = 20;
    public const int MaxSize = 100;
    public int Page { get; }
    public int PageSize { get; }
    public int Skip => ((Page - 1) * PageSize);

    private Pagination(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public static Pagination From(int? page, int? pageSize)
    {
        int pageNormalized = Math.Max(1, page ?? 1);
        int pageSizeNormalized = Math.Clamp(pageSize ?? DefaultSize, 1, MaxSize);
        Pagination pagination = new(pageNormalized, pageSizeNormalized);

        return pagination;
    }
}