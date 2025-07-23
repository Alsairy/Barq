using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class AITaskConfiguration : IEntityTypeConfiguration<AITask>
{
    public void Configure(EntityTypeBuilder<AITask> builder)
    {
        builder.ToTable("AITasks");

        builder.HasKey(at => at.Id);

        builder.Property(at => at.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(at => at.Description)
            .HasMaxLength(1000);

        builder.Property(at => at.TaskType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(at => at.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(at => at.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(at => at.InputData)
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.OutputData)
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.AIProvider)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(at => at.AIModel)
            .HasMaxLength(100);

        builder.Property(at => at.Cost)
            .HasColumnType("decimal(18,2)");

        builder.Property(at => at.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(at => at.Parameters)
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.Configuration)
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.ReviewComments)
            .HasColumnType("nvarchar(max)");

        builder.Property(at => at.TenantId)
            .IsRequired();

        builder.Property(at => at.CreatedAt)
            .IsRequired();

        builder.Property(at => at.UpdatedAt);

        builder.Ignore(at => at.UserId);

        builder.HasOne(at => at.Project)
            .WithMany(p => p.AITasks)
            .HasForeignKey(at => at.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(at => at.Sprint)
            .WithMany(s => s.AITasks)
            .HasForeignKey(at => at.SprintId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(at => at.UserStory)
            .WithMany(us => us.AITasks)
            .HasForeignKey(at => at.UserStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(at => at.ParentTask)
            .WithMany(at => at.ChildTasks)
            .HasForeignKey(at => at.ParentTaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(at => at.WorkflowInstance)
            .WithMany(wi => wi.AITasks)
            .HasForeignKey(at => at.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(at => at.Assignee)
            .WithMany()
            .HasForeignKey(at => at.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(at => at.TenantId);
        builder.HasIndex(at => at.ProjectId);
        builder.HasIndex(at => at.SprintId);
        builder.HasIndex(at => at.UserStoryId);
        builder.HasIndex(at => at.ParentTaskId);
        builder.HasIndex(at => at.WorkflowInstanceId);
        builder.HasIndex(at => at.AssigneeId);
        builder.HasIndex(at => at.Status);
        builder.HasIndex(at => at.Priority);
        builder.HasIndex(at => at.TaskType);
        builder.HasIndex(at => new { at.TenantId, at.Status });
        builder.HasIndex(at => new { at.ProjectId, at.Status });
    }
}
