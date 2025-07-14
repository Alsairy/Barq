using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BARQ.Infrastructure.Middleware;

public class ApiMonitoringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiMonitoringMiddleware> _logger;

    public ApiMonitoringMiddleware(RequestDelegate next, ILogger<ApiMonitoringMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
        
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
        
        var requestDetails = new
        {
            CorrelationId = correlationId,
            TenantId = tenantId,
            Method = context.Request.Method,
            Path = context.Request.Path.Value,
            QueryString = context.Request.QueryString.Value,
            UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
            RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
            Timestamp = DateTime.UtcNow,
            UserId = context.User?.Identity?.Name,
            ContentType = context.Request.ContentType,
            ContentLength = context.Request.ContentLength
        };

        _logger.LogInformation("API Request Started: {RequestDetails}", JsonSerializer.Serialize(requestDetails));

        string requestBody = null;
        if (context.Request.Method == "POST" || context.Request.Method == "PUT")
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        Exception exception = null;
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            var responseBody = string.Empty;
            if (responseBodyStream.Length > 0)
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }

            var responseDetails = new
            {
                CorrelationId = correlationId,
                TenantId = tenantId,
                StatusCode = context.Response.StatusCode,
                ContentType = context.Response.ContentType,
                ContentLength = responseBodyStream.Length,
                Duration = stopwatch.ElapsedMilliseconds,
                Success = context.Response.StatusCode < 400,
                Exception = exception?.Message,
                ExceptionType = exception?.GetType().Name,
                ResponseSize = responseBodyStream.Length,
                Timestamp = DateTime.UtcNow
            };

            var logLevel = exception != null ? LogLevel.Error : 
                          context.Response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            
            _logger.Log(logLevel, "API Request Completed: {ResponseDetails}", JsonSerializer.Serialize(responseDetails));

            if (stopwatch.ElapsedMilliseconds > 5000) // 5 seconds threshold
            {
                _logger.LogWarning("Slow API Request: {Method} {Path} took {Duration}ms", 
                    context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            }

            var analyticsData = new
            {
                CorrelationId = correlationId,
                TenantId = tenantId,
                Endpoint = $"{context.Request.Method} {context.Request.Path}",
                StatusCode = context.Response.StatusCode,
                Duration = stopwatch.ElapsedMilliseconds,
                RequestSize = context.Request.ContentLength ?? 0,
                ResponseSize = responseBodyStream.Length,
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserId = context.User?.Identity?.Name,
                Timestamp = DateTime.UtcNow,
                Success = context.Response.StatusCode < 400,
                ErrorType = exception?.GetType().Name,
                ErrorMessage = exception?.Message
            };

            _logger.LogInformation("API_ANALYTICS: {AnalyticsData}", JsonSerializer.Serialize(analyticsData));
        }
    }
}

public static class ApiMonitoringMiddlewareExtensions
{
    public static IApplicationBuilder UseApiMonitoring(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiMonitoringMiddleware>();
    }
}
