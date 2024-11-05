using Microsoft.AspNetCore.WebUtilities;
using TaskManager.Domain.Extensions;

namespace TaskManager.Domain.Services;

public class UriService : IUriService
{
    private string Uri { get; set; }

    public void SetUri(string scheme, string host, string route)
    {
        Uri = string.Concat(scheme, "://", host, route);
    }

    public Uri GetUrlPage(PaginationFilter filter)
    {
        var modifiedUri =
            QueryHelpers.AddQueryString(new Uri(Uri).ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}