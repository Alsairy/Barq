using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class IntegrationRequest
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Method { get; set; } = "POST";
    /// <summary>
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// </summary>
    public string? Body { get; set; }
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// </summary>
    public IntegrationProtocol Protocol { get; set; } = IntegrationProtocol.REST;
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// </summary>
    public string? CorrelationId { get; set; }
    /// <summary>
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// </summary>
public class IntegrationResponse
{
    /// <summary>
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Body { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    
    /// <summary>
    /// </summary>
    public string? EndpointId { get; set; }
}

/// <summary>
/// </summary>
public class IntegrationEndpoint
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IntegrationProtocol Protocol { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? AuthenticationType { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, string> AuthenticationConfig { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    /// <summary>
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    /// <summary>
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public DateTime? LastHealthCheck { get; set; }
    /// <summary>
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// </summary>
public class IntegrationHealthStatus
{
    /// <summary>
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool IsHealthy { get; set; }
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// </summary>
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public long ResponseTimeMs { get; set; }
    /// <summary>
    /// </summary>
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();
}

/// <summary>
/// </summary>
public class IntegrationLog
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Method { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// </summary>
public class IntegrationMessage
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string MessageType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    /// <summary>
    /// </summary>
    public int RetryCount { get; set; } = 0;
    /// <summary>
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Pending;
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// </summary>
    public string? CorrelationId { get; set; }
}

/// <summary>
/// </summary>
public class MessageTransformationResult
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// </summary>
    public string? TransformedContent { get; set; }
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// </summary>
    public string SourceFormat { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string TargetFormat { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime TransformedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// </summary>
public class QueueStatus
{
    /// <summary>
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public int PendingMessages { get; set; }
    /// <summary>
    /// </summary>
    public int ProcessingMessages { get; set; }
    /// <summary>
    /// </summary>
    public int CompletedMessages { get; set; }
    /// <summary>
    /// </summary>
    public int FailedMessages { get; set; }
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// </summary>
public class IntegrationEvent
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IntegrationEventLevel Level { get; set; } = IntegrationEventLevel.Info;
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// </summary>
public class IntegrationMetrics
{
    /// <summary>
    /// </summary>
    public int TotalRequests { get; set; }
    /// <summary>
    /// </summary>
    public int SuccessfulRequests { get; set; }
    /// <summary>
    /// </summary>
    public int FailedRequests { get; set; }
    /// <summary>
    /// </summary>
    public double SuccessRate { get; set; }
    /// <summary>
    /// </summary>
    public double AverageResponseTime { get; set; }
    /// <summary>
    /// </summary>
    public Dictionary<string, int> EndpointUsage { get; set; } = new();
    /// <summary>
    /// </summary>
    public Dictionary<string, int> ErrorCounts { get; set; } = new();
    /// <summary>
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ToDate { get; set; }
}

/// <summary>
/// </summary>
public class IntegrationAlert
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string RuleId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// </summary>
    public bool IsResolved { get; set; } = false;
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
}

/// <summary>
/// </summary>
public class IntegrationAlertRule
{
    /// <summary>
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Condition { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// </summary>
    public string? EndpointId { get; set; }
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// </summary>
public class IntegrationHealthDashboard
{
    /// <summary>
    /// </summary>
    public int TotalEndpoints { get; set; }
    /// <summary>
    /// </summary>
    public int HealthyEndpoints { get; set; }
    /// <summary>
    /// </summary>
    public int UnhealthyEndpoints { get; set; }
    /// <summary>
    /// </summary>
    public int ActiveAlerts { get; set; }
    /// <summary>
    /// </summary>
    public double OverallHealthScore { get; set; }
    /// <summary>
    /// </summary>
    public IEnumerable<IntegrationEndpoint> RecentlyFailedEndpoints { get; set; } = new List<IntegrationEndpoint>();
    /// <summary>
    /// </summary>
    public IEnumerable<IntegrationAlert> CriticalAlerts { get; set; } = new List<IntegrationAlert>();
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
