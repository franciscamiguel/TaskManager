using TaskManager.Domain.Extensions;

namespace TaskManager.Domain.Services;

public interface IUriService
{
    public void SetUri(string scheme, string host, string route);

    public Uri GetUrlPage(PaginationFilter filter);
}