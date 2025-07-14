using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BARQ.Infrastructure.Caching;

public class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IMemoryCache _memoryCache;
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<CachingService> _logger;
    private readonly CachingOptions _options;
    private readonly CacheStatistics _statistics;

    public CachingService(
        IDistributedCache distributedCache,
        IMemoryCache memoryCache,
        IConnectionMultiplexer redis,
        ILogger<CachingService> logger,
        IOptions<CachingOptions> options)
    {
        _distributedCache = distributedCache;
        _memoryCache = memoryCache;
        _redisDatabase = redis.GetDatabase();
        _logger = logger;
        _options = options.Value;
        _statistics = new CacheStatistics { LastUpdated = DateTime.UtcNow };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            if (_memoryCache.TryGetValue(key, out T? memoryValue))
            {
                _statistics.HitCount++;
                _logger.LogDebug("Cache hit (memory): {Key}", key);
                return memoryValue;
            }

            var distributedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (distributedValue != null)
            {
                var deserializedValue = JsonSerializer.Deserialize<T>(distributedValue);
                
                var memoryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.MemoryCacheExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(_options.MemoryCacheSlidingExpirationMinutes),
                    Size = 1
                };
                _memoryCache.Set(key, deserializedValue, memoryOptions);
                
                _statistics.HitCount++;
                _logger.LogDebug("Cache hit (distributed): {Key}", key);
                return deserializedValue;
            }

            _statistics.MissCount++;
            _logger.LogDebug("Cache miss: {Key}", key);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache value for key: {Key}", key);
            _statistics.MissCount++;
            return null;
        }
        finally
        {
            stopwatch.Stop();
            _statistics.AverageResponseTime = TimeSpan.FromMilliseconds(
                (_statistics.AverageResponseTime.TotalMilliseconds + stopwatch.ElapsedMilliseconds) / 2);
        }
    }

    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            if (_memoryCache.TryGetValue(key, out string? memoryValue))
            {
                _statistics.HitCount++;
                return memoryValue;
            }

            var distributedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (distributedValue != null)
            {
                var memoryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.MemoryCacheExpirationMinutes),
                    Size = 1
                };
                _memoryCache.Set(key, distributedValue, memoryOptions);
                
                _statistics.HitCount++;
                return distributedValue;
            }

            _statistics.MissCount++;
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting string cache value for key: {Key}", key);
            _statistics.MissCount++;
            return null;
        }
        finally
        {
            stopwatch.Stop();
            _statistics.AverageResponseTime = TimeSpan.FromMilliseconds(
                (_statistics.AverageResponseTime.TotalMilliseconds + stopwatch.ElapsedMilliseconds) / 2);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions();
            
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.DefaultExpirationMinutes);
            }

            await _distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);

            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Math.Min(_options.MemoryCacheExpirationMinutes, 
                    expiration?.TotalMinutes ?? _options.DefaultExpirationMinutes)),
                Size = 1
            };
            _memoryCache.Set(key, value, memoryOptions);

            _logger.LogDebug("Cache set: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
        }
    }

    public async Task SetStringAsync(string key, string value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.DefaultExpirationMinutes);
            }

            await _distributedCache.SetStringAsync(key, value, options, cancellationToken);

            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Math.Min(_options.MemoryCacheExpirationMinutes, 
                    expiration?.TotalMinutes ?? _options.DefaultExpirationMinutes)),
                Size = 1
            };
            _memoryCache.Set(key, value, memoryOptions);

            _logger.LogDebug("Cache string set: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting string cache value for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            _memoryCache.Remove(key);
            _logger.LogDebug("Cache removed: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache value for key: {Key}", key);
        }
    }

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        try
        {
            var server = _redisDatabase.Multiplexer.GetServer(_redisDatabase.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern);
            
            var tasks = keys.Select(key => _redisDatabase.KeyDeleteAsync(key)).ToArray();
            await Task.WhenAll(tasks);
            
            _logger.LogDebug("Cache pattern removed: {Pattern}, Keys: {KeyCount}", pattern, tasks.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache pattern: {Pattern}", pattern);
        }
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_memoryCache.TryGetValue(key, out _))
            {
                return true;
            }

            return await _redisDatabase.KeyExistsAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking cache existence for key: {Key}", key);
            return false;
        }
    }

    public async Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RefreshAsync(key, cancellationToken);
            _logger.LogDebug("Cache refreshed: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing cache for key: {Key}", key);
        }
    }

    public async Task<T?> GetTenantAsync<T>(string tenantId, string key, CancellationToken cancellationToken = default) where T : class
    {
        var tenantKey = $"tenant:{tenantId}:{key}";
        return await GetAsync<T>(tenantKey, cancellationToken);
    }

    public async Task SetTenantAsync<T>(string tenantId, string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        var tenantKey = $"tenant:{tenantId}:{key}";
        await SetAsync(tenantKey, value, expiration, cancellationToken);
    }

    public async Task RemoveTenantAsync(string tenantId, string key, CancellationToken cancellationToken = default)
    {
        var tenantKey = $"tenant:{tenantId}:{key}";
        await RemoveAsync(tenantKey, cancellationToken);
    }

    public async Task RemoveTenantPatternAsync(string tenantId, string pattern, CancellationToken cancellationToken = default)
    {
        var tenantPattern = $"tenant:{tenantId}:{pattern}";
        await RemoveByPatternAsync(tenantPattern, cancellationToken);
    }

    public async Task WarmCacheAsync(string key, Func<Task<object>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!await ExistsAsync(key, cancellationToken))
            {
                var value = await factory();
                if (value != null)
                {
                    var serializedValue = JsonSerializer.Serialize(value);
                    await SetStringAsync(key, serializedValue, expiration, cancellationToken);
                    _logger.LogDebug("Cache warmed: {Key}", key);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error warming cache for key: {Key}", key);
        }
    }

    public async Task InvalidateTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        try
        {
            var pattern = $"*:tag:{tag}:*";
            await RemoveByPatternAsync(pattern, cancellationToken);
            _logger.LogDebug("Cache tag invalidated: {Tag}", tag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating cache tag: {Tag}", tag);
        }
    }

    public async Task InvalidateMultipleAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = keys.Select(key => RemoveAsync(key, cancellationToken));
            await Task.WhenAll(tasks);
            _logger.LogDebug("Multiple cache keys invalidated: {KeyCount}", keys.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating multiple cache keys");
        }
    }

    public async Task<CacheStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var info = await _redisDatabase.ExecuteAsync("INFO", "memory");
            var memoryInfo = info.ToString();
            
            var usedMemoryMatch = Regex.Match(memoryInfo, @"used_memory:(\d+)");
            if (usedMemoryMatch.Success && long.TryParse(usedMemoryMatch.Groups[1].Value, out var usedMemory))
            {
                _statistics.CacheSize = usedMemory;
            }

            var keyspaceInfo = await _redisDatabase.ExecuteAsync("INFO", "keyspace");
            var keyspaceStr = keyspaceInfo.ToString();
            var keysMatch = Regex.Match(keyspaceStr, @"keys=(\d+)");
            if (keysMatch.Success && int.TryParse(keysMatch.Groups[1].Value, out var keyCount))
            {
                _statistics.KeyCount = keyCount;
            }

            _statistics.LastUpdated = DateTime.UtcNow;
            return _statistics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache statistics");
            return _statistics;
        }
    }

    public async Task<CacheHealth> GetHealthAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var testKey = $"health_check_{Guid.NewGuid()}";
            var testValue = "health_test";
            
            await SetStringAsync(testKey, testValue, TimeSpan.FromSeconds(30), cancellationToken);
            var retrievedValue = await GetStringAsync(testKey, cancellationToken);
            await RemoveAsync(testKey, cancellationToken);
            
            stopwatch.Stop();
            
            var isHealthy = testValue == retrievedValue;
            return new CacheHealth
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                ResponseTime = stopwatch.Elapsed,
                Details = new Dictionary<string, object>
                {
                    ["TestSuccessful"] = isHealthy,
                    ["ResponseTimeMs"] = stopwatch.ElapsedMilliseconds,
                    ["Statistics"] = await GetStatisticsAsync(cancellationToken)
                }
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new CacheHealth
            {
                IsHealthy = false,
                Status = "Unhealthy",
                ResponseTime = stopwatch.Elapsed,
                ErrorMessage = ex.Message,
                Details = new Dictionary<string, object>
                {
                    ["Exception"] = ex.GetType().Name,
                    ["ResponseTimeMs"] = stopwatch.ElapsedMilliseconds
                }
            };
        }
    }
}

public class CachingOptions
{
    public int DefaultExpirationMinutes { get; set; } = 60;
    public int MemoryCacheExpirationMinutes { get; set; } = 15;
    public int MemoryCacheSlidingExpirationMinutes { get; set; } = 5;
    public long MemoryCacheSizeLimit { get; set; } = 100 * 1024 * 1024; // 100MB
    public bool EnableCompression { get; set; } = true;
    public bool EnableStatistics { get; set; } = true;
}
