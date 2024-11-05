namespace TaskManager.Domain.Extensions;

public class PagerResponse<T>(int pageNumber, int pageSize, IEnumerable<T> data) : Response<T>(data)
{
    public int PageNumber { get; private set; } = pageNumber;
    public int PageSize { get; private set; } = pageSize;
    public int TotalPages { get; private set; }
    public int TotalRows { get; private set; }

    public Uri NextPage { get; private set; }
    public Uri PreviousPage { get; private set; }
    public Uri FirstPage { get; private set; }
    public Uri LastPage { get; private set; }

    internal void SetPagesUri(Uri next, Uri previous, Uri first, Uri last)
    {
        NextPage = next;
        PreviousPage = previous;
        FirstPage = first;
        LastPage = last;
    }

    internal void SetTotalPagesAndTotalRows(int totalPages, int totalRows)
    {
        TotalPages = totalPages;
        TotalRows = totalRows;
    }
}