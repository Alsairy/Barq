using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ToTable("Sprints");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.Goal)
            .HasMaxLength(500);

        builder.Property(s => s.Capacity)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Velocity)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Budget)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.AICost)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.RetrospectiveNotes)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.TenantId)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        builder.HasOne(s => s.Project)
            .WithMany(p => p.Sprints)
            .HasForeignKey(s => s.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProjectId);
        builder.HasIndex(s => s.TenantId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.SprintNumber);
        builder.HasIndex(s => new { s.ProjectId, s.SprintNumber })
            .IsUnique();
    }
}
