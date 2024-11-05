using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Contracts.Repositories;
using TaskManager.Domain.Models.TaskAggregate.Dtos;
using TaskManager.Domain.Models.TaskAggregate.Entities;
using TaskManager.Persistence.Repositories.Generics;

namespace TaskManager.Persistence.Repositories;

public class TaskRepository(DataContext context) : RepositoryGeneric<Tasks>(context), ITaskRepository
{
    private readonly DataContext _context = context;

    public IQueryable<Tasks> GetAllTasksAsync()
    {
        return DbSet.FromSqlRaw("EXEC usp_GetAllTasks");
    }

    public Tasks GetTaskByIdAsync(int id)
    {
        var idParam = new SqlParameter("@Id", id);
        return DbSet
            .FromSqlRaw("EXEC usp_GetTaskById @Id", idParam)
            .AsEnumerable()
            .FirstOrDefault();
    }

    public async Task<int> AddTaskAsync(RegisterTaskDto registerTaskDto)
    {
        SetParameters(registerTaskDto, out var titleParam, out var descriptionParam, out var isCompletedParam);
        var idParam = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };

        await _context.Database.ExecuteSqlRawAsync("EXEC usp_AddTask @Title, @Description, @IsCompleted, @Id OUTPUT",
            titleParam, descriptionParam, isCompletedParam, idParam);

        return idParam.Value != null ? (int)idParam.Value : 0;
    }

    public async Task<int> UpdateTaskAsync(int id, RegisterTaskDto registerTaskDto)
    {
        var idParam = new SqlParameter("@Id", id);
        SetParameters(registerTaskDto, out var titleParam, out var descriptionParam, out var isCompletedParam);

        return await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpdateTask @Id, @Title, @Description, @IsCompleted",
            idParam, titleParam, descriptionParam, isCompletedParam);
    }

    public async Task<int> RemoveTaskAsync(int id)
    {
        var idParam = new SqlParameter("@Id", id);
        return await _context.Database.ExecuteSqlRawAsync("EXEC usp_DeleteTask @Id", idParam);
    }

    public async Task UpdateAttachmentKeyAsync(int taskId, string attachmentKey)
    {
        var idParam = new SqlParameter("@Id", taskId);
        var awsKeyParam = new SqlParameter("@AttachmentKey", attachmentKey ?? (object)DBNull.Value);

        await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpdateAwsKey @Id, @AttachmentKey", idParam, awsKeyParam);
    }

    private static void SetParameters(
        RegisterTaskDto registerTaskDto,
        out SqlParameter titleParam,
        out SqlParameter descriptionParam,
        out SqlParameter isCompletedParam)
    {
        titleParam = new SqlParameter("@Title", registerTaskDto.Title);
        descriptionParam = new SqlParameter("@Description", registerTaskDto.Description ?? (object)DBNull.Value);
        isCompletedParam = new SqlParameter("@IsCompleted", registerTaskDto.IsCompleted);
    }
}