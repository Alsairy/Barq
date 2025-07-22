using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class UserStoryConfiguration : IEntityTypeConfiguration<UserStory>
{
    public void Configure(EntityTypeBuilder<UserStory> builder)
    {
        builder.ToTable("UserStories");

        builder.HasKey(us => us.Id);

        builder.Property(us => us.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(us => us.Description)
            .HasMaxLength(2000);

        builder.Property(us => us.AsAUser)
            .HasMaxLength(1000);

        builder.Property(us => us.IWant)
            .HasMaxLength(1000);

        builder.Property(us => us.SoThat)
            .HasMaxLength(1000);

        builder.Property(us => us.AcceptanceCriteria)
            .HasColumnType("nvarchar(max)");

        builder.Property(us => us.BusinessRules)
            .HasColumnType("nvarchar(max)");

        builder.Property(us => us.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(us => us.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(us => us.Epic)
            .HasMaxLength(200);

        builder.Property(us => us.Labels)
            .HasColumnType("nvarchar(max)");

        builder.Property(us => us.AIGenerationMetadata)
            .HasColumnType("nvarchar(max)");

        builder.Property(us => us.TenantId)
            .IsRequired();

        builder.Property(us => us.CreatedAt)
            .IsRequired();

        builder.Property(us => us.UpdatedAt);

        builder.HasOne(us => us.Project)
            .WithMany(p => p.UserStories)
            .HasForeignKey(us => us.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(us => us.Sprint)
            .WithMany(s => s.UserStories)
            .HasForeignKey(us => us.SprintId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(us => us.Assignee)
            .WithMany()
            .HasForeignKey(us => us.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(us => us.ProjectId);
        builder.HasIndex(us => us.SprintId);
        builder.HasIndex(us => us.AssigneeId);
        builder.HasIndex(us => us.TenantId);
        builder.HasIndex(us => us.Status);
        builder.HasIndex(us => us.Priority);
    }
}
