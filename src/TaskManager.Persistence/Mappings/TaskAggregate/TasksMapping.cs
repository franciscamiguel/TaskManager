using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Models.TaskAggregate.Entities;

namespace TaskManager.Persistence.Mappings.TaskAggregate;

public class TasksMapping : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {
        builder.ToTable(nameof(Tasks));

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.Property(t => t.Title)
            .IsRequired();
        builder.Property(t => t.Description)
            .IsRequired();
        builder.Property(t => t.IsCompleted)
            .IsRequired();
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        builder.Property(t => t.AttachmentKey);
    }
}