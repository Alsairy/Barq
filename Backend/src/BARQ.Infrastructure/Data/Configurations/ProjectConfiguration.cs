using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.ProjectKey)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.ProjectType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Budget)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ActualCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ProgressPercentage)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.TechnologyStack)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.RepositoryUrl)
            .HasMaxLength(500);

        builder.Property(p => p.RepositoryBranch)
            .HasMaxLength(100);

        builder.Property(p => p.LogoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Settings)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.AIConfiguration)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.DesignSystemUrl)
            .HasMaxLength(500);

        builder.Property(p => p.TenantId)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        builder.Ignore(p => p.EndDate);

        builder.HasOne(p => p.ProjectOwner)
            .WithMany()
            .HasForeignKey(p => p.ProjectOwnerId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(p => p.Organization)
            .WithMany(o => o.Projects)
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.ProjectKey)
            .IsUnique();
        builder.HasIndex(p => p.ProjectOwnerId);
        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.Priority);
    }
}
