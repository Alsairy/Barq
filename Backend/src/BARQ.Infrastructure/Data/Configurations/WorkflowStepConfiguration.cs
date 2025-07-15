using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        builder.ToTable("WorkflowSteps");

        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ws => ws.Description)
            .HasMaxLength(1000);

        builder.Property(ws => ws.StepType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ws => ws.Order)
            .IsRequired();

        builder.Property(ws => ws.Configuration)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.InputSchema)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.OutputSchema)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.ValidationRules)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.RetryConfiguration)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.ErrorHandling)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.ExecutionConditions)
            .HasColumnType("nvarchar(max)");

        builder.Property(ws => ws.RequiresApproval)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ws => ws.AllowParallelExecution)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ws => ws.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ws => ws.TimeoutMinutes);

        builder.Property(ws => ws.TenantId)
            .IsRequired();

        builder.Property(ws => ws.CreatedAt)
            .IsRequired();

        builder.Property(ws => ws.UpdatedAt);

        builder.HasOne(ws => ws.WorkflowTemplate)
            .WithMany()
            .HasForeignKey(ws => ws.WorkflowTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ws => ws.ParentStep)
            .WithMany(ws => ws.ChildSteps)
            .HasForeignKey(ws => ws.ParentStepId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ws => ws.WorkflowTemplateId);
        builder.HasIndex(ws => ws.ParentStepId);
        builder.HasIndex(ws => new { ws.WorkflowTemplateId, ws.Order });
        builder.HasIndex(ws => ws.StepType);
        builder.HasIndex(ws => ws.TenantId);
        builder.HasIndex(ws => ws.IsActive);
    }
}
