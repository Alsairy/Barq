using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.Data;

public class BarqDbContextFactory : IDesignTimeDbContextFactory<BarqDbContext>
{
    public BarqDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.Testing.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BarqDbContext>();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Database=barq_test;Username=postgres;Password=postgres";

        if (connectionString.Contains("Host=") || connectionString.Contains("Server=") && connectionString.Contains("Database="))
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        var tenantProvider = new DesignTimeTenantProvider();
        
        return new BarqDbContext(optionsBuilder.Options, tenantProvider);
    }
}

public class DesignTimeTenantProvider : ITenantProvider
{
    public Guid GetTenantId()
    {
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    public string GetTenantName()
    {
        return "DefaultTenant";
    }

    public void SetTenantId(Guid tenantId)
    {
    }
}
