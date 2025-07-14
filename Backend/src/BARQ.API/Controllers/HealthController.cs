using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BARQ.Shared.DTOs;
using System.Diagnostics;

namespace BARQ.API.Controllers;

/// <summary>
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetHealth()
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();
            
            var response = new
            {
                Status = healthReport.Status.ToString(),
                TotalDuration = healthReport.TotalDuration.TotalMilliseconds,
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.TotalMilliseconds,
                    Description = entry.Value.Description,
                    Data = entry.Value.Data,
                    Exception = entry.Value.Exception?.Message
                })
            };

            return Ok(new ApiResponse<object>
            {
                Success = healthReport.Status == HealthStatus.Healthy,
                Data = response,
                Message = $"System health status: {healthReport.Status}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = $"Health check failed: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("detailed")]
    public async Task<ActionResult<ApiResponse<object>>> GetDetailedHealth()
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();
            
            var detailedResponse = new
            {
                Status = healthReport.Status.ToString(),
                TotalDuration = healthReport.TotalDuration.TotalMilliseconds,
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                WorkingSet = Environment.WorkingSet,
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.TotalMilliseconds,
                    Description = entry.Value.Description,
                    Tags = entry.Value.Tags,
                    Data = entry.Value.Data,
                    Exception = entry.Value.Exception != null ? new
                    {
                        Message = entry.Value.Exception.Message,
                        StackTrace = entry.Value.Exception.StackTrace,
                        Type = entry.Value.Exception.GetType().Name
                    } : null
                })
            };

            return Ok(new ApiResponse<object>
            {
                Success = healthReport.Status == HealthStatus.Healthy,
                Data = detailedResponse,
                Message = $"Detailed system health status: {healthReport.Status}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = $"Detailed health check failed: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{checkName}")]
    public async Task<ActionResult<ApiResponse<object>>> GetSpecificHealth(string checkName)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();
            
            if (!healthReport.Entries.TryGetValue(checkName, out var healthEntry))
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Health check '{checkName}' not found"
                });
            }

            var response = new
            {
                Name = checkName,
                Status = healthEntry.Status.ToString(),
                Duration = healthEntry.Duration.TotalMilliseconds,
                Description = healthEntry.Description,
                Tags = healthEntry.Tags,
                Data = healthEntry.Data,
                Exception = healthEntry.Exception != null ? new
                {
                    Message = healthEntry.Exception.Message,
                    Type = healthEntry.Exception.GetType().Name
                } : null
            };

            return Ok(new ApiResponse<object>
            {
                Success = healthEntry.Status == HealthStatus.Healthy,
                Data = response,
                Message = $"Health check '{checkName}' status: {healthEntry.Status}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = $"Health check for '{checkName}' failed: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("ready")]
    public async Task<ActionResult<ApiResponse<object>>> GetReadiness()
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(check => 
                check.Tags.Contains("ready") || check.Tags.Contains("readiness"));
            
            var response = new
            {
                Status = healthReport.Status.ToString(),
                Ready = healthReport.Status == HealthStatus.Healthy,
                Timestamp = DateTime.UtcNow,
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.TotalMilliseconds
                })
            };

            var statusCode = healthReport.Status == HealthStatus.Healthy ? 200 : 503;
            
            return StatusCode(statusCode, new ApiResponse<object>
            {
                Success = healthReport.Status == HealthStatus.Healthy,
                Data = response,
                Message = $"System readiness: {(healthReport.Status == HealthStatus.Healthy ? "Ready" : "Not Ready")}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new ApiResponse<object>
            {
                Success = false,
                Message = $"Readiness check failed: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("live")]
    public async Task<ActionResult<ApiResponse<object>>> GetLiveness()
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(check => 
                check.Tags.Contains("live") || check.Tags.Contains("liveness"));
            
            var response = new
            {
                Status = healthReport.Status.ToString(),
                Alive = healthReport.Status != HealthStatus.Unhealthy,
                Timestamp = DateTime.UtcNow,
                Uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime(),
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.TotalMilliseconds
                })
            };

            var statusCode = healthReport.Status != HealthStatus.Unhealthy ? 200 : 503;
            
            return StatusCode(statusCode, new ApiResponse<object>
            {
                Success = healthReport.Status != HealthStatus.Unhealthy,
                Data = response,
                Message = $"System liveness: {(healthReport.Status != HealthStatus.Unhealthy ? "Alive" : "Unhealthy")}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new ApiResponse<object>
            {
                Success = false,
                Message = $"Liveness check failed: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("startup")]
    public async Task<ActionResult<ApiResponse<object>>> GetStartup()
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(check => 
                check.Tags.Contains("startup"));
            
            var response = new
            {
                Status = healthReport.Status.ToString(),
                Started = healthReport.Status == HealthStatus.Healthy,
                Timestamp = DateTime.UtcNow,
                StartTime = Process.GetCurrentProcess().StartTime.ToUniversalTime(),
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.TotalMilliseconds
                })
            };

            var statusCode = healthReport.Status == HealthStatus.Healthy ? 200 : 503;
            
            return StatusCode(statusCode, new ApiResponse<object>
            {
                Success = healthReport.Status == HealthStatus.Healthy,
                Data = response,
                Message = $"System startup: {(healthReport.Status == HealthStatus.Healthy ? "Complete" : "In Progress")}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new ApiResponse<object>
            {
                Success = false,
                Message = $"Startup check failed: {ex.Message}"
            });
        }
    }
}
