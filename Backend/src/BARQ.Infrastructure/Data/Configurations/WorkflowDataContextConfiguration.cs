using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class WorkflowDataContextConfiguration : IEntityTypeConfiguration<WorkflowDataContext>
{
    public void Configure(EntityTypeBuilder<WorkflowDataContext> builder)
    {
        builder.ToTable("WorkflowDataContexts");

        builder.HasKey(wdc => wdc.Id);

        builder.Property(wdc => wdc.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(wdc => wdc.Scope)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("workflow");

        builder.Property(wdc => wdc.Data)
            .IsRequired()
            .HasColumnType("nvarchar(max)")
            .HasDefaultValue("{}");

        builder.Property(wdc => wdc.DataSchema)
            .HasColumnType("nvarchar(max)");

        builder.Property(wdc => wdc.EncryptionKeyId)
            .HasMaxLength(100);

        builder.Property(wdc => wdc.IsEncrypted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(wdc => wdc.AccessPermissions)
            .HasColumnType("nvarchar(max)");

        builder.Property(wdc => wdc.ValidationRules)
            .HasColumnType("nvarchar(max)");

        builder.Property(wdc => wdc.TransformationRules)
            .HasColumnType("nvarchar(max)");

        builder.Property(wdc => wdc.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(wdc => wdc.TenantId)
            .IsRequired();

        builder.Property(wdc => wdc.CreatedAt)
            .IsRequired();

        builder.Property(wdc => wdc.UpdatedAt);

        builder.HasOne(wdc => wdc.WorkflowInstance)
            .WithMany()
            .HasForeignKey(wdc => wdc.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wdc => wdc.WorkflowStep)
            .WithMany()
            .HasForeignKey(wdc => wdc.WorkflowStepId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wdc => wdc.ParentContext)
            .WithMany(wdc => wdc.ChildContexts)
            .HasForeignKey(wdc => wdc.ParentContextId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(wdc => wdc.WorkflowInstanceId);
        builder.HasIndex(wdc => wdc.WorkflowStepId);
        builder.HasIndex(wdc => wdc.ParentContextId);
        builder.HasIndex(wdc => wdc.TenantId);
        builder.HasIndex(wdc => wdc.Scope);
        builder.HasIndex(wdc => wdc.IsActive);
        builder.HasIndex(wdc => new { wdc.WorkflowInstanceId, wdc.Scope });
    }
}
