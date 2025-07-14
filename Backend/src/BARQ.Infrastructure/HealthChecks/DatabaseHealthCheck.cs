using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;

namespace BARQ.Infrastructure.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly BarqDbContext _context;

    public DatabaseHealthCheck(BarqDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            await _context.Database.CanConnectAsync(cancellationToken);
            
            var userCount = await _context.Users.CountAsync(cancellationToken);
            var organizationCount = await _context.Organizations.CountAsync(cancellationToken);
            
            stopwatch.Stop();
            
            var data = new Dictionary<string, object>
            {
                ["ConnectionTime"] = stopwatch.ElapsedMilliseconds,
                ["UserCount"] = userCount,
                ["OrganizationCount"] = organizationCount,
                ["DatabaseProvider"] = _context.Database.ProviderName,
                ["ConnectionString"] = _context.Database.GetConnectionString()?.Substring(0, Math.Min(50, _context.Database.GetConnectionString()?.Length ?? 0)) + "..."
            };

            if (stopwatch.ElapsedMilliseconds > 5000) // 5 seconds threshold
            {
                return HealthCheckResult.Degraded(
                    $"Database connection is slow ({stopwatch.ElapsedMilliseconds}ms)",
                    data: data);
            }

            return HealthCheckResult.Healthy(
                $"Database is healthy (Response time: {stopwatch.ElapsedMilliseconds}ms)",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Database health check failed: {ex.Message}",
                ex,
                new Dictionary<string, object>
                {
                    ["Exception"] = ex.GetType().Name,
                    ["Message"] = ex.Message
                });
        }
    }
}
