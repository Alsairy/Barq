using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace BARQ.Infrastructure.Analytics;

public interface IApiAnalyticsService
{
    Task TrackApiCallAsync(ApiCallMetrics metrics);
    Task<ApiUsageReport> GetUsageReportAsync(string tenantId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<ApiEndpointStats>> GetEndpointStatsAsync(string tenantId, TimeSpan period);
    Task<IEnumerable<ApiErrorStats>> GetErrorStatsAsync(string tenantId, TimeSpan period);
    Task<ApiPerformanceMetrics> GetPerformanceMetricsAsync(string tenantId, TimeSpan period);
}

public class ApiAnalyticsService : IApiAnalyticsService
{
    private readonly ILogger<ApiAnalyticsService> _logger;
    private readonly IDistributedCache _cache;

    public ApiAnalyticsService(ILogger<ApiAnalyticsService> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task TrackApiCallAsync(ApiCallMetrics metrics)
    {
        try
        {
            var cacheKey = $"api_metrics:{metrics.TenantId}:{DateTime.UtcNow:yyyy-MM-dd-HH}";
            var existingData = await _cache.GetStringAsync(cacheKey);
            
            var hourlyMetrics = existingData != null 
                ? JsonSerializer.Deserialize<List<ApiCallMetrics>>(existingData) 
                : new List<ApiCallMetrics>();
            
            hourlyMetrics.Add(metrics);
            
            var serializedData = JsonSerializer.Serialize(hourlyMetrics);
            await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });

            await UpdateEndpointCountersAsync(metrics);
            
            if (!metrics.Success)
            {
                await UpdateErrorCountersAsync(metrics);
            }

            _logger.LogDebug("API call metrics tracked for tenant {TenantId}, endpoint {Endpoint}", 
                metrics.TenantId, metrics.Endpoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to track API call metrics for tenant {TenantId}", metrics.TenantId);
        }
    }

    public async Task<ApiUsageReport> GetUsageReportAsync(string tenantId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var report = new ApiUsageReport
            {
                TenantId = tenantId,
                StartDate = startDate,
                EndDate = endDate,
                TotalRequests = 0,
                SuccessfulRequests = 0,
                FailedRequests = 0,
                AverageResponseTime = 0,
                TopEndpoints = new List<ApiEndpointStats>(),
                ErrorBreakdown = new List<ApiErrorStats>()
            };

            var current = startDate.Date;
            while (current <= endDate.Date)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    var cacheKey = $"api_metrics:{tenantId}:{current:yyyy-MM-dd}-{hour:D2}";
                    var data = await _cache.GetStringAsync(cacheKey);
                    
                    if (data != null)
                    {
                        var metrics = JsonSerializer.Deserialize<List<ApiCallMetrics>>(data);
                        foreach (var metric in metrics)
                        {
                            report.TotalRequests++;
                            if (metric.Success)
                                report.SuccessfulRequests++;
                            else
                                report.FailedRequests++;
                            
                            report.AverageResponseTime += metric.Duration;
                        }
                    }
                }
                current = current.AddDays(1);
            }

            if (report.TotalRequests > 0)
            {
                report.AverageResponseTime /= report.TotalRequests;
            }

            report.TopEndpoints = await GetEndpointStatsAsync(tenantId, endDate - startDate);
            report.ErrorBreakdown = await GetErrorStatsAsync(tenantId, endDate - startDate);

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate usage report for tenant {TenantId}", tenantId);
            throw;
        }
    }

    public async Task<IEnumerable<ApiEndpointStats>> GetEndpointStatsAsync(string tenantId, TimeSpan period)
    {
        try
        {
            var endpointStatsKey = $"endpoint_stats:{tenantId}";
            var data = await _cache.GetStringAsync(endpointStatsKey);
            
            if (data != null)
            {
                return JsonSerializer.Deserialize<List<ApiEndpointStats>>(data) ?? new List<ApiEndpointStats>();
            }

            return new List<ApiEndpointStats>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get endpoint stats for tenant {TenantId}", tenantId);
            return new List<ApiEndpointStats>();
        }
    }

    public async Task<IEnumerable<ApiErrorStats>> GetErrorStatsAsync(string tenantId, TimeSpan period)
    {
        try
        {
            var errorStatsKey = $"error_stats:{tenantId}";
            var data = await _cache.GetStringAsync(errorStatsKey);
            
            if (data != null)
            {
                return JsonSerializer.Deserialize<List<ApiErrorStats>>(data) ?? new List<ApiErrorStats>();
            }

            return new List<ApiErrorStats>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get error stats for tenant {TenantId}", tenantId);
            return new List<ApiErrorStats>();
        }
    }

    public async Task<ApiPerformanceMetrics> GetPerformanceMetricsAsync(string tenantId, TimeSpan period)
    {
        try
        {
            var performanceKey = $"performance_metrics:{tenantId}";
            var data = await _cache.GetStringAsync(performanceKey);
            
            if (data != null)
            {
                return JsonSerializer.Deserialize<ApiPerformanceMetrics>(data) ?? new ApiPerformanceMetrics();
            }

            return new ApiPerformanceMetrics
            {
                TenantId = tenantId,
                AverageResponseTime = 0,
                MedianResponseTime = 0,
                P95ResponseTime = 0,
                P99ResponseTime = 0,
                ThroughputPerSecond = 0,
                ErrorRate = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get performance metrics for tenant {TenantId}", tenantId);
            throw;
        }
    }

    private async Task UpdateEndpointCountersAsync(ApiCallMetrics metrics)
    {
        try
        {
            var endpointStatsKey = $"endpoint_stats:{metrics.TenantId}";
            var data = await _cache.GetStringAsync(endpointStatsKey);
            
            var endpointStats = data != null 
                ? JsonSerializer.Deserialize<List<ApiEndpointStats>>(data) 
                : new List<ApiEndpointStats>();

            var existingStat = endpointStats.FirstOrDefault(s => s.Endpoint == metrics.Endpoint);
            if (existingStat != null)
            {
                existingStat.RequestCount++;
                existingStat.AverageResponseTime = (existingStat.AverageResponseTime + metrics.Duration) / 2;
                existingStat.LastAccessed = metrics.Timestamp;
            }
            else
            {
                endpointStats.Add(new ApiEndpointStats
                {
                    Endpoint = metrics.Endpoint,
                    RequestCount = 1,
                    AverageResponseTime = metrics.Duration,
                    LastAccessed = metrics.Timestamp
                });
            }

            var serializedData = JsonSerializer.Serialize(endpointStats);
            await _cache.SetStringAsync(endpointStatsKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update endpoint counters for {Endpoint}", metrics.Endpoint);
        }
    }

    private async Task UpdateErrorCountersAsync(ApiCallMetrics metrics)
    {
        try
        {
            var errorStatsKey = $"error_stats:{metrics.TenantId}";
            var data = await _cache.GetStringAsync(errorStatsKey);
            
            var errorStats = data != null 
                ? JsonSerializer.Deserialize<List<ApiErrorStats>>(data) 
                : new List<ApiErrorStats>();

            var existingStat = errorStats.FirstOrDefault(s => s.StatusCode == metrics.StatusCode && s.Endpoint == metrics.Endpoint);
            if (existingStat != null)
            {
                existingStat.Count++;
                existingStat.LastOccurrence = metrics.Timestamp;
            }
            else
            {
                errorStats.Add(new ApiErrorStats
                {
                    Endpoint = metrics.Endpoint,
                    StatusCode = metrics.StatusCode,
                    ErrorType = metrics.ErrorType,
                    Count = 1,
                    LastOccurrence = metrics.Timestamp
                });
            }

            var serializedData = JsonSerializer.Serialize(errorStats);
            await _cache.SetStringAsync(errorStatsKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update error counters for {Endpoint}", metrics.Endpoint);
        }
    }
}

public class ApiCallMetrics
{
    public string CorrelationId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long Duration { get; set; }
    public long RequestSize { get; set; }
    public long ResponseSize { get; set; }
    public string UserAgent { get; set; } = string.Empty;
    public string RemoteIpAddress { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool Success { get; set; }
    public string ErrorType { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class ApiUsageReport
{
    public string TenantId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long TotalRequests { get; set; }
    public long SuccessfulRequests { get; set; }
    public long FailedRequests { get; set; }
    public double AverageResponseTime { get; set; }
    public IEnumerable<ApiEndpointStats> TopEndpoints { get; set; } = new List<ApiEndpointStats>();
    public IEnumerable<ApiErrorStats> ErrorBreakdown { get; set; } = new List<ApiErrorStats>();
}

public class ApiEndpointStats
{
    public string Endpoint { get; set; } = string.Empty;
    public long RequestCount { get; set; }
    public double AverageResponseTime { get; set; }
    public DateTime LastAccessed { get; set; }
}

public class ApiErrorStats
{
    public string Endpoint { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string ErrorType { get; set; } = string.Empty;
    public long Count { get; set; }
    public DateTime LastOccurrence { get; set; }
}

public class ApiPerformanceMetrics
{
    public string TenantId { get; set; } = string.Empty;
    public double AverageResponseTime { get; set; }
    public double MedianResponseTime { get; set; }
    public double P95ResponseTime { get; set; }
    public double P99ResponseTime { get; set; }
    public double ThroughputPerSecond { get; set; }
    public double ErrorRate { get; set; }
}
