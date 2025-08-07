using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.Data;

public class BarqDbContextFactory : IDesignTimeDbContextFactory<BarqDbContext>
{
    public BarqDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BARQ.API"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var builder = new DbContextOptionsBuilder<BarqDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        builder.UseNpgsql(connectionString, options => options.EnableRetryOnFailure());

        var tenantProvider = new DesignTimeTenantProvider();
        return new BarqDbContext(builder.Options, tenantProvider);
    }
}

public class DesignTimeTenantProvider : ITenantProvider
{
    private Guid _tenantId = Guid.Empty;
    private string _tenantName = "Design Time";

    public Guid GetTenantId() => _tenantId;
    public string GetTenantName() => _tenantName;
    public bool IsMultiTenant() => false;
    public void ClearTenantContext() 
    { 
        _tenantId = Guid.Empty;
        _tenantName = string.Empty;
    }
    public Guid GetCurrentUserId() => Guid.Empty;
    public void SetTenantId(Guid tenantId) => _tenantId = tenantId;
    public void SetTenantName(string tenantName) => _tenantName = tenantName;
}
