using TaskManager.Domain.Contracts.Repositories.Generics;
using TaskManager.Domain.Models.TaskAggregate.Dtos;
using TaskManager.Domain.Models.TaskAggregate.Entities;

namespace TaskManager.Domain.Contracts.Repositories;

public interface ITaskRepository : IRepositoryGeneric<Tasks>
{
    /// <summary>
    ///     Consulta tarefas
    /// </summary>
    /// <returns></returns>
    IQueryable<Tasks> GetAllTasksAsync();

    /// <summary>
    ///     Consulta uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Tasks GetTaskByIdAsync(int id);

    /// <summary>
    ///     Registra uma tarefa
    /// </summary>
    /// <param name="registerTaskDto"></param>
    /// <returns></returns>
    Task<int> AddTaskAsync(RegisterTaskDto registerTaskDto);

    /// <summary>
    ///     Atualiza uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <param name="registerTaskDto"></param>
    /// <returns></returns>
    Task<int> UpdateTaskAsync(int id, RegisterTaskDto registerTaskDto);

    /// <summary>
    ///     Remove uma tarefa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> RemoveTaskAsync(int id);

    /// <summary>
    ///     Atualiza a chave do anexo
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="attachmentKey"></param>
    /// <returns></returns>
    Task UpdateAttachmentKeyAsync(int taskId, string attachmentKey);
}