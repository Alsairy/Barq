using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class LdapConfigurationConfiguration : IEntityTypeConfiguration<LdapConfiguration>
{
    public void Configure(EntityTypeBuilder<LdapConfiguration> builder)
    {
        builder.ToTable("LdapConfigurations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.TenantId)
            .IsRequired();

        builder.Property(l => l.Host)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(l => l.Port)
            .IsRequired();

        builder.Property(l => l.BaseDn)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.BindDn)
            .HasMaxLength(1000);

        builder.Property(l => l.BindPassword)
            .HasMaxLength(500);

        builder.Property(l => l.UserSearchFilter)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.GroupSearchFilter)
            .HasMaxLength(1000);

        builder.Property(l => l.UserDnPattern)
            .HasMaxLength(1000);

        builder.Property(l => l.EmailAttribute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.FirstNameAttribute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.LastNameAttribute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.DisplayNameAttribute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.GroupMembershipAttribute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.DefaultRole)
            .HasMaxLength(100);

        builder.Property(l => l.GroupRoleMappings)
            .HasColumnType("nvarchar(max)");

        builder.Property(l => l.ValidationError)
            .HasMaxLength(1000);

        builder.HasOne(l => l.Tenant)
            .WithMany()
            .HasForeignKey(l => l.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.TenantId)
            .IsUnique()
            .HasDatabaseName("IX_LdapConfigurations_TenantId");

        builder.HasIndex(l => l.Host)
            .HasDatabaseName("IX_LdapConfigurations_Host");

        builder.HasIndex(l => l.IsEnabled)
            .HasDatabaseName("IX_LdapConfigurations_IsEnabled");
    }
}
