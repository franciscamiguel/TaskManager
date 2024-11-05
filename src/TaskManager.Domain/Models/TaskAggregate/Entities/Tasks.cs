using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Models.TaskAggregate.Entities;

public class Tasks
{
    protected Tasks()
    {
    }

    public Tasks(string title, string description)
    {
        Title = title;
        Description = description;

        SetIsCompleted(true);
    }

    [Key]
    public int Id { get; init; }

    [Column(TypeName = "nvarchar(50)")]
    public string Title { get; private set; }

    [Column(TypeName = "nvarchar(2000)")]
    public string Description { get; private set; }

    public bool IsCompleted { get; private set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; private init; } = DateTime.Now;

    [Column(TypeName = "nvarchar(2000)")]
    public string AttachmentKey { get; }

    private void SetIsCompleted(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }
}