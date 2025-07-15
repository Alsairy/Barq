using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowStepExecutionConfiguration : IEntityTypeConfiguration<WorkflowStepExecution>
{
    public void Configure(EntityTypeBuilder<WorkflowStepExecution> builder)
    {
        builder.ToTable("WorkflowStepExecutions");

        builder.HasKey(wse => wse.Id);

        builder.Property(wse => wse.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(wse => wse.InputData)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.OutputData)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(wse => wse.ErrorDetails)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.ExecutionContext)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.ExecutionLogs)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.PerformanceMetrics)
            .HasColumnType("nvarchar(max)");

        builder.Property(wse => wse.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(wse => wse.MaxRetries)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(wse => wse.TenantId)
            .IsRequired();

        builder.Property(wse => wse.CreatedAt)
            .IsRequired();

        builder.Property(wse => wse.UpdatedAt);

        builder.HasOne(wse => wse.WorkflowInstance)
            .WithMany()
            .HasForeignKey(wse => wse.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wse => wse.WorkflowStep)
            .WithMany(ws => ws.StepExecutions)
            .HasForeignKey(wse => wse.WorkflowStepId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(wse => wse.ExecutedBy)
            .WithMany()
            .HasForeignKey(wse => wse.ExecutedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(wse => wse.AssignedTo)
            .WithMany()
            .HasForeignKey(wse => wse.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(wse => wse.WorkflowInstanceId);
        builder.HasIndex(wse => wse.WorkflowStepId);
        builder.HasIndex(wse => wse.Status);
        builder.HasIndex(wse => wse.TenantId);
        builder.HasIndex(wse => wse.ExecutedById);
        builder.HasIndex(wse => wse.AssignedToId);
        builder.HasIndex(wse => new { wse.WorkflowInstanceId, wse.Status });
    }
}
