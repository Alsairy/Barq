using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BARQ.Infrastructure.Security;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly RateLimitConfiguration _config;

    public RateLimitingMiddleware(
        RequestDelegate next, 
        IDistributedCache cache, 
        ILogger<RateLimitingMiddleware> logger,
        RateLimitConfiguration config)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_config.EnableRateLimiting)
        {
            await _next(context);
            return;
        }

        var clientId = GetClientIdentifier(context);
        var endpoint = GetEndpointIdentifier(context);

        var rateLimitInfo = await GetRateLimitInfoAsync(clientId, endpoint);

        if (await IsRateLimitExceededAsync(rateLimitInfo))
        {
            await HandleRateLimitExceededAsync(context, rateLimitInfo);
            return;
        }

        await UpdateRateLimitAsync(clientId, endpoint, rateLimitInfo);

        AddRateLimitHeaders(context, rateLimitInfo);

        await _next(context);
    }

    private string GetClientIdentifier(HttpContext context)
    {
        var clientId = context.Request.Headers["X-Client-ID"].FirstOrDefault();
        if (!string.IsNullOrEmpty(clientId))
            return $"client:{clientId}";

        var userId = context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
            return $"user:{userId}";

        var ipAddress = GetClientIpAddress(context);
        return $"ip:{ipAddress}";
    }

    private string GetEndpointIdentifier(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? "/";
        var method = context.Request.Method.ToUpperInvariant();
        return $"{method}:{path}";
    }

    private async Task<RateLimitInfo> GetRateLimitInfoAsync(string clientId, string endpoint)
    {
        var key = $"ratelimit:{clientId}:{endpoint}";
        var cachedData = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(cachedData))
        {
            return new RateLimitInfo
            {
                Key = key,
                RequestCount = 0,
                WindowStart = DateTime.UtcNow,
                IsBlocked = false,
                BlockedUntil = null
            };
        }

        return JsonSerializer.Deserialize<RateLimitInfo>(cachedData) ?? new RateLimitInfo { Key = key };
    }

    private async Task<bool> IsRateLimitExceededAsync(RateLimitInfo rateLimitInfo)
    {
        if (rateLimitInfo.IsBlocked && rateLimitInfo.BlockedUntil > DateTime.UtcNow)
        {
            return true;
        }

        if (rateLimitInfo.WindowStart.AddSeconds(_config.WindowSizeSeconds) < DateTime.UtcNow)
        {
            rateLimitInfo.RequestCount = 0;
            rateLimitInfo.WindowStart = DateTime.UtcNow;
            rateLimitInfo.IsBlocked = false;
            rateLimitInfo.BlockedUntil = null;
        }

        return rateLimitInfo.RequestCount >= _config.MaxRequestsPerWindow;
    }

    private async Task UpdateRateLimitAsync(string clientId, string endpoint, RateLimitInfo rateLimitInfo)
    {
        rateLimitInfo.RequestCount++;

        if (rateLimitInfo.RequestCount >= _config.MaxRequestsPerWindow)
        {
            rateLimitInfo.IsBlocked = true;
            rateLimitInfo.BlockedUntil = DateTime.UtcNow.AddSeconds(_config.BlockDurationSeconds);
            
            _logger.LogWarning("Rate limit exceeded for client {ClientId} on endpoint {Endpoint}. Blocked until {BlockedUntil}",
                clientId, endpoint, rateLimitInfo.BlockedUntil);
        }

        var serializedData = JsonSerializer.Serialize(rateLimitInfo);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_config.WindowSizeSeconds + _config.BlockDurationSeconds)
        };

        await _cache.SetStringAsync(rateLimitInfo.Key, serializedData, options);
    }

    private async Task HandleRateLimitExceededAsync(HttpContext context, RateLimitInfo rateLimitInfo)
    {
        context.Response.StatusCode = 429; // Too Many Requests
        context.Response.ContentType = "application/json";

        var retryAfter = rateLimitInfo.BlockedUntil?.Subtract(DateTime.UtcNow).TotalSeconds ?? _config.BlockDurationSeconds;
        context.Response.Headers.Add("Retry-After", ((int)retryAfter).ToString());

        var response = new
        {
            error = "Rate limit exceeded",
            message = "Too many requests. Please try again later.",
            retryAfter = (int)retryAfter,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private void AddRateLimitHeaders(HttpContext context, RateLimitInfo rateLimitInfo)
    {
        var remaining = Math.Max(0, _config.MaxRequestsPerWindow - rateLimitInfo.RequestCount);
        var resetTime = rateLimitInfo.WindowStart.AddSeconds(_config.WindowSizeSeconds);

        context.Response.Headers.Add("X-RateLimit-Limit", _config.MaxRequestsPerWindow.ToString());
        context.Response.Headers.Add("X-RateLimit-Remaining", remaining.ToString());
        context.Response.Headers.Add("X-RateLimit-Reset", ((DateTimeOffset)resetTime).ToUnixTimeSeconds().ToString());
    }

    private string GetClientIpAddress(HttpContext context)
    {
        var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = context.Connection.RemoteIpAddress?.ToString();
        }
        return ipAddress ?? "Unknown";
    }
}

public class RateLimitInfo
{
    public string Key { get; set; } = string.Empty;
    public int RequestCount { get; set; }
    public DateTime WindowStart { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? BlockedUntil { get; set; }
}

public class RateLimitConfiguration
{
    public bool EnableRateLimiting { get; set; } = true;
    public int MaxRequestsPerWindow { get; set; } = 100;
    public int WindowSizeSeconds { get; set; } = 60;
    public int BlockDurationSeconds { get; set; } = 300; // 5 minutes
    public bool EnableAdaptiveRateLimiting { get; set; } = true;
    public double SuspiciousThresholdMultiplier { get; set; } = 0.8;
}
