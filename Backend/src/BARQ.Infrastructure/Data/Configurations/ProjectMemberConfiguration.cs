using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Role)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pm => pm.JoinedAt)
            .IsRequired();

        builder.Property(pm => pm.IsActive)
            .IsRequired();

        builder.Property(pm => pm.AllocationPercentage)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(pm => pm.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMemberships)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(pm => pm.TenantId)
            .IsRequired();

        builder.HasIndex(pm => pm.TenantId);
        builder.HasIndex(pm => pm.ProjectId);
        builder.HasIndex(pm => pm.UserId);
    }
}
