using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.Data
{
    public class BarqDbContextFactory : IDesignTimeDbContextFactory<BarqDbContext>
    {
        private class DesignTimeTenantProvider : ITenantProvider
        {
            private Guid _tenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            private string _tenantName = "DefaultTenant";
            public Guid GetTenantId() => _tenantId;
            public void SetTenantId(Guid tenantId) => _tenantId = tenantId;
            public string GetTenantName() => _tenantName;
            public void SetTenantName(string tenantName) => _tenantName = tenantName;
            public bool IsMultiTenant() => false;
            public void ClearTenantContext() { _tenantId = Guid.Empty; _tenantName = string.Empty; }
            public Guid GetCurrentUserId() => Guid.Empty;
        }

        public BarqDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<BarqDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new BarqDbContext(optionsBuilder.Options, new DesignTimeTenantProvider());
        }
    }
}
