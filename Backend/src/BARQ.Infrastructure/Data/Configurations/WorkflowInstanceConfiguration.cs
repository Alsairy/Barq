using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(w => w.WorkflowTemplate)
            .WithMany(wt => wt.WorkflowInstances)
            .HasForeignKey(w => w.WorkflowTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Project)
            .WithMany()
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

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
    }
}
