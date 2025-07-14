using Microsoft.Extensions.Caching.Distributed;

namespace BARQ.Infrastructure.Caching;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class;
    Task SetStringAsync(string key, string value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
    Task RefreshAsync(string key, CancellationToken cancellationToken = default);
    
    Task<T?> GetTenantAsync<T>(string tenantId, string key, CancellationToken cancellationToken = default) where T : class;
    Task SetTenantAsync<T>(string tenantId, string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class;
    Task RemoveTenantAsync(string tenantId, string key, CancellationToken cancellationToken = default);
    Task RemoveTenantPatternAsync(string tenantId, string pattern, CancellationToken cancellationToken = default);
    
    Task WarmCacheAsync(string key, Func<Task<object>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task InvalidateTagAsync(string tag, CancellationToken cancellationToken = default);
    Task InvalidateMultipleAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);
    
    Task<CacheStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
    Task<CacheHealth> GetHealthAsync(CancellationToken cancellationToken = default);
}

public class CacheStatistics
{
    public long HitCount { get; set; }
    public long MissCount { get; set; }
    public double HitRatio => HitCount + MissCount > 0 ? (double)HitCount / (HitCount + MissCount) : 0;
    public long TotalRequests => HitCount + MissCount;
    public TimeSpan AverageResponseTime { get; set; }
    public long CacheSize { get; set; }
    public int KeyCount { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CacheHealth
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public TimeSpan ResponseTime { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public Dictionary<string, object> Details { get; set; } = new();
}
