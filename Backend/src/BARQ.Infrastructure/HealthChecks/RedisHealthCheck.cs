using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace BARQ.Infrastructure.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDistributedCache _cache;

    public RedisHealthCheck(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var testKey = $"health_check_{Guid.NewGuid()}";
            var testValue = "health_check_value";
            var testValueBytes = Encoding.UTF8.GetBytes(testValue);
            
            await _cache.SetAsync(testKey, testValueBytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            }, cancellationToken);
            
            var retrievedBytes = await _cache.GetAsync(testKey, cancellationToken);
            var retrievedValue = retrievedBytes != null ? Encoding.UTF8.GetString(retrievedBytes) : null;
            
            await _cache.RemoveAsync(testKey, cancellationToken);
            
            stopwatch.Stop();
            
            var data = new Dictionary<string, object>
            {
                ["ResponseTime"] = stopwatch.ElapsedMilliseconds,
                ["TestKey"] = testKey,
                ["ValueMatches"] = testValue == retrievedValue
            };

            if (testValue != retrievedValue)
            {
                return HealthCheckResult.Unhealthy(
                    "Redis value mismatch - cache may be corrupted",
                    data: data);
            }

            if (stopwatch.ElapsedMilliseconds > 2000) // 2 seconds threshold
            {
                return HealthCheckResult.Degraded(
                    $"Redis is responding slowly ({stopwatch.ElapsedMilliseconds}ms)",
                    data: data);
            }

            return HealthCheckResult.Healthy(
                $"Redis is healthy (Response time: {stopwatch.ElapsedMilliseconds}ms)",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Redis health check failed: {ex.Message}",
                ex,
                new Dictionary<string, object>
                {
                    ["Exception"] = ex.GetType().Name,
                    ["Message"] = ex.Message
                });
        }
    }
}
