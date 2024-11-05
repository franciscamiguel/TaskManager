using TaskManager.Domain.Extensions;
using TaskManager.Domain.Models.TaskAggregate.Dtos;
using TaskManager.Domain.Models.TaskAggregate.Entities;

namespace TaskManager.Domain.Contracts.Entities;

public interface ITaskService
{
    public PagerResponse<Tasks> Get(PaginationFilter paginationFilter, RequestFilter requestFilter);
    public Tasks Get(int id);
    public Task<int> RegisterAsync(RegisterTaskDto registerTaskDto);
    public Task<bool> UpdateAsync(int id, RegisterTaskDto registerTaskDto);
    public Task<bool> RemoveAsync(int id);
    Task UpdateAttachmentKeyAsync(int taskId, string attachmentKey);
}