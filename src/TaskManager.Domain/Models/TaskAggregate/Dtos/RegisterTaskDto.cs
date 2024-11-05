namespace TaskManager.Domain.Models.TaskAggregate.Dtos;

public record RegisterTaskDto(string Title, string Description, bool IsCompleted);