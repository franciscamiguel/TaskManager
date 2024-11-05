namespace TaskManager.Domain.Extensions;

public record PaginationFilter(int PageNumber, int PageSize)
{
    public int PageNumber { get; set; } = PageNumber;
    public int PageSize { get; set; } = PageSize;
}