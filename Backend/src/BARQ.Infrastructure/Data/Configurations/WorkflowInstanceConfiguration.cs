using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
    {
        builder.ToTable("WorkflowInstances");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.Property(w => w.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(w => w.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(w => w.WorkflowData)
            .HasColumnType("nvarchar(max)");

        builder.Property(w => w.ExecutionContext)
            .HasColumnType("nvarchar(max)");

        builder.Property(w => w.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(w => w.ErrorDetails)
            .HasColumnType("nvarchar(max)");

        builder.Property(w => w.PerformanceMetrics)
            .HasColumnType("nvarchar(max)");

        builder.Property(w => w.TenantId)
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt);

        builder.HasOne(w => w.WorkflowTemplate)
            .WithMany(wt => wt.WorkflowInstances)
            .HasForeignKey(w => w.WorkflowTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Project)
            .WithMany()
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Sprint)
            .WithMany()
            .HasForeignKey(w => w.SprintId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.UserStory)
            .WithMany()
            .HasForeignKey(w => w.UserStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.CurrentAssignee)
            .WithMany(u => u.AssignedWorkflows)
            .HasForeignKey(w => w.CurrentAssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Initiator)
            .WithMany()
            .HasForeignKey(w => w.InitiatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.AITasks)
            .WithOne(at => at.WorkflowInstance)
            .HasForeignKey(at => at.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => w.TenantId);
        builder.HasIndex(w => w.WorkflowTemplateId);
        builder.HasIndex(w => w.Status);
        builder.HasIndex(w => w.Priority);
        builder.HasIndex(w => w.ProjectId);
        builder.HasIndex(w => w.SprintId);
        builder.HasIndex(w => w.UserStoryId);
        builder.HasIndex(w => w.CurrentAssigneeId);
        builder.HasIndex(w => w.InitiatorId);
        builder.HasIndex(w => new { w.TenantId, w.Status });
        builder.HasIndex(w => new { w.WorkflowTemplateId, w.Status });
        builder.HasIndex(w => new { w.ProjectId, w.Status });
    }
}
