using TaskManager.Domain.Services;

namespace TaskManager.Domain.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    ///     Get Query with paginator
    /// </summary>
    /// <param name="query"></param>
    /// <param name="filter"></param>
    /// <param name="uriService"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PagerResponse<T> PagerResponse<T>(
        this IEnumerable<T> query,
        PaginationFilter filter,
        IUriService uriService)
    {
        if (query is null)
            return new PagerResponse<T>(filter.PageNumber, filter.PageSize, null);

        SetPageNumberAndSize(filter);

        var paged = query.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize);

        var response = InitializePagerResponse(query, filter, ref paged);

        var totalRecords = query.Count();
        var totalPages = (decimal)totalRecords / filter.PageSize;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

        response.SetPagesUri(
            filter.PageNumber >= 1 && filter.PageNumber < roundedTotalPages
                ? uriService.GetUrlPage(new PaginationFilter(filter.PageNumber + 1, filter.PageSize))
                : null,
            filter.PageNumber - 1 >= 1 && filter.PageNumber <= roundedTotalPages
                ? uriService.GetUrlPage(new PaginationFilter(filter.PageNumber - 1, filter.PageSize))
                : null,
            uriService.GetUrlPage(new PaginationFilter(1, filter.PageSize)),
            uriService.GetUrlPage(new PaginationFilter(roundedTotalPages, filter.PageSize))
        );

        response.SetTotalPagesAndTotalRows(roundedTotalPages, totalRecords);

        return response;
    }

    #region Private Methods

    private static void SetPageNumberAndSize(PaginationFilter filter)
    {
        if (filter.PageNumber < 1)
            filter.PageNumber = 1;

        if (filter.PageSize < 1)
            filter.PageSize = 500;
    }

    private static PagerResponse<T> InitializePagerResponse<T>(
        IEnumerable<T> query,
        PaginationFilter filter,
        ref IEnumerable<T> paged)
    {
        if (paged.Any() || filter.PageNumber is 1)
            return new PagerResponse<T>(filter.PageNumber, filter.PageSize, [.. paged]);

        paged = query.Skip(filter.PageSize * (filter.PageNumber - 2)).Take(filter.PageSize);
        filter.PageNumber--;

        return new PagerResponse<T>(filter.PageNumber, filter.PageSize, [.. paged]);
    }

    #endregion Private Methods
}