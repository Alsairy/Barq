using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.RoleName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ur => ur.Description)
            .HasMaxLength(500);

        builder.Property(ur => ur.Permissions)
            .HasColumnType("nvarchar(max)");

        builder.Property(ur => ur.IsActive)
            .IsRequired();

        builder.Property(ur => ur.TenantId)
            .IsRequired();

        builder.Property(ur => ur.CreatedAt)
            .IsRequired();

        builder.Property(ur => ur.UpdatedAt);

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ur => ur.UserId);
        builder.HasIndex(ur => ur.RoleId);
        builder.HasIndex(ur => ur.TenantId);
        builder.HasIndex(ur => ur.IsActive);
    }
}
