using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class SsoConfigurationConfiguration : IEntityTypeConfiguration<SsoConfiguration>
{
    public void Configure(EntityTypeBuilder<SsoConfiguration> builder)
    {
        builder.ToTable("SsoConfigurations");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId)
            .IsRequired();

        builder.Property(s => s.Provider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.ProviderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ConfigurationJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.EntityId)
            .HasMaxLength(500);

        builder.Property(s => s.SsoUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.LogoutUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.Certificate)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.ClientId)
            .HasMaxLength(200);

        builder.Property(s => s.ClientSecret)
            .HasMaxLength(500);

        builder.Property(s => s.Scopes)
            .HasMaxLength(500);

        builder.Property(s => s.Authority)
            .HasMaxLength(1000);

        builder.Property(s => s.CallbackUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.AttributeMappings)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.DefaultRole)
            .HasMaxLength(100);

        builder.Property(s => s.ValidationError)
            .HasMaxLength(1000);

        builder.HasOne(s => s.Tenant)
            .WithMany()
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.TenantId, s.Provider })
            .IsUnique()
            .HasDatabaseName("IX_SsoConfigurations_TenantId_Provider");

        builder.HasIndex(s => s.TenantId)
            .HasDatabaseName("IX_SsoConfigurations_TenantId");

        builder.HasIndex(s => s.Provider)
            .HasDatabaseName("IX_SsoConfigurations_Provider");

        builder.HasIndex(s => s.IsEnabled)
            .HasDatabaseName("IX_SsoConfigurations_IsEnabled");
    }
}
