using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowTemplateConfiguration : IEntityTypeConfiguration<WorkflowTemplate>
{
    public void Configure(EntityTypeBuilder<WorkflowTemplate> builder)
    {
        builder.HasKey(wt => wt.Id);

        builder.Property(wt => wt.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(wt => wt.Description)
            .HasMaxLength(1000);

        builder.Property(wt => wt.Version)
            .HasMaxLength(20);

        builder.Property(wt => wt.WorkflowDefinition)
            .IsRequired();

        builder.Property(wt => wt.ApprovalSteps)
            .IsRequired();

        builder.Ignore(wt => wt.Organization);

        builder.HasMany(wt => wt.WorkflowInstances)
            .WithOne(wi => wi.WorkflowTemplate)
            .HasForeignKey(wi => wi.WorkflowTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(wt => wt.TenantId);
        builder.HasIndex(wt => wt.WorkflowType);
        builder.HasIndex(wt => wt.IsActive);
        builder.HasIndex(wt => wt.IsDefault);
    }
}
