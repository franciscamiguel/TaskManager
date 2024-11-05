using Microsoft.Extensions.Logging;
using TaskManager.Domain.Contracts.Entities;
using TaskManager.Domain.Contracts.Repositories;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Models.TaskAggregate.Dtos;
using TaskManager.Domain.Models.TaskAggregate.Entities;
using TaskManager.Domain.Notifier;
using TaskManager.Domain.Services;

namespace TaskManager.Domain.Models.TaskAggregate.Services;

public class TaskService(
    ITaskRepository taskRepository,
    INotifierMessage notifierMessage,
    IUriService uriService,
    ILogger<TaskService> logger) : ITaskService
{
    public PagerResponse<Tasks> Get(PaginationFilter paginationFilter, RequestFilter requestFilter)
    {
        uriService.SetUri(requestFilter.Scheme, requestFilter.Host, requestFilter.Path);

        var response = taskRepository.GetAllTasksAsync()
            .PagerResponse(paginationFilter, uriService);

        return response;
    }

    public Tasks Get(int id)
    {
        return taskRepository.GetTaskByIdAsync(id);
    }

    public async Task<int> RegisterAsync(RegisterTaskDto registerTaskDto)
    {
        try
        {
            var id = await taskRepository.AddTaskAsync(registerTaskDto);

            if (id > 0) return id;

            notifierMessage.Add("Erro ao salvar");
            return 0;
        }
        catch (Exception ex)
        {
            var message = $"{ex.Message} '-' {ex.InnerException?.Message}";
            logger.LogError(ex, message);
            notifierMessage.Add(message);
            return 0;
        }
    }

    public async Task<bool> UpdateAsync(int id, RegisterTaskDto registerTaskDto)
    {
        try
        {
            var updated = await taskRepository.UpdateTaskAsync(id, registerTaskDto);
            if (updated > 0) return true;

            notifierMessage.Add("Erro ao atualizar");
            return false;
        }
        catch (Exception ex)
        {
            var message = $"{ex.Message} '-' {ex.InnerException?.Message}";
            logger.LogError(ex, message);
            notifierMessage.Add(message);
            return false;
        }
    }

    public async Task<bool> RemoveAsync(int id)
    {
        try
        {
            var deleted = await taskRepository.RemoveTaskAsync(id);

            if (deleted > 0) return true;

            notifierMessage.Add("Erro ao deletar");
            return false;
        }
        catch (Exception ex)
        {
            var message = $"{ex.Message} '-' {ex.InnerException?.Message}";
            logger.LogError(ex, message);
            notifierMessage.Add(message);
            return false;
        }
    }

    public async Task UpdateAttachmentKeyAsync(int taskId, string attachmentKey)
    {
        try
        {
            await taskRepository.UpdateAttachmentKeyAsync(taskId, attachmentKey);
        }
        catch (Exception ex)
        {
            var message = $"{ex.Message} '-' {ex.InnerException?.Message}";
            logger.LogError(ex, message);
            notifierMessage.Add(message);
        }
    }
}