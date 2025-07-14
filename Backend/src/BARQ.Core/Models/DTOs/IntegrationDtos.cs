using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

public class IntegrationRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EndpointId { get; set; } = string.Empty;
    public string Method { get; set; } = "POST";
    public string Path { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public string? Body { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
    public IntegrationProtocol Protocol { get; set; } = IntegrationProtocol.REST;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TenantId { get; set; }
    public string? CorrelationId { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
}

public class IntegrationResponse
{
    public string RequestId { get; set; } = string.Empty;
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Body { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    public long ProcessingTimeMs { get; set; }
    public string? EndpointId { get; set; }
}

public class IntegrationEndpoint
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public IntegrationProtocol Protocol { get; set; }
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    public string? AuthenticationType { get; set; }
    public Dictionary<string, string> AuthenticationConfig { get; set; } = new();
    public bool IsActive { get; set; } = true;
    public int TimeoutSeconds { get; set; } = 30;
    public int RetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    public Guid TenantId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastHealthCheck { get; set; }
    public bool IsHealthy { get; set; } = true;
}

public class IntegrationHealthStatus
{
    public string EndpointId { get; set; } = string.Empty;
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    public long ResponseTimeMs { get; set; }
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();
}

public class IntegrationLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RequestId { get; set; } = string.Empty;
    public string EndpointId { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public long ProcessingTimeMs { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TenantId { get; set; }
}

public class IntegrationMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string QueueName { get; set; } = string.Empty;
    public string MessageType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; } = 0;
    public int MaxRetries { get; set; } = 3;
    public string? ErrorMessage { get; set; }
    public MessageStatus Status { get; set; } = MessageStatus.Pending;
    public Guid TenantId { get; set; }
    public string? CorrelationId { get; set; }
}

public class MessageTransformationResult
{
    public bool Success { get; set; }
    public string? TransformedContent { get; set; }
    public string? ErrorMessage { get; set; }
    public string SourceFormat { get; set; } = string.Empty;
    public string TargetFormat { get; set; } = string.Empty;
    public DateTime TransformedAt { get; set; } = DateTime.UtcNow;
}

public class QueueStatus
{
    public string QueueName { get; set; } = string.Empty;
    public int PendingMessages { get; set; }
    public int ProcessingMessages { get; set; }
    public int CompletedMessages { get; set; }
    public int FailedMessages { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsHealthy { get; set; } = true;
}

public class IntegrationEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventType { get; set; } = string.Empty;
    public string EndpointId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IntegrationEventLevel Level { get; set; } = IntegrationEventLevel.Info;
    public Dictionary<string, object> Data { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TenantId { get; set; }
}

public class IntegrationMetrics
{
    public int TotalRequests { get; set; }
    public int SuccessfulRequests { get; set; }
    public int FailedRequests { get; set; }
    public double SuccessRate { get; set; }
    public double AverageResponseTime { get; set; }
    public Dictionary<string, int> EndpointUsage { get; set; } = new();
    public Dictionary<string, int> ErrorCounts { get; set; } = new();
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class IntegrationAlert
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RuleId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IntegrationAlertSeverity Severity { get; set; }
    public string EndpointId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    public bool IsResolved { get; set; } = false;
    public Dictionary<string, object> Data { get; set; } = new();
}

public class IntegrationAlertRule
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public IntegrationAlertSeverity Severity { get; set; }
    public bool IsActive { get; set; } = true;
    public string? EndpointId { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class IntegrationHealthDashboard
{
    public int TotalEndpoints { get; set; }
    public int HealthyEndpoints { get; set; }
    public int UnhealthyEndpoints { get; set; }
    public int ActiveAlerts { get; set; }
    public double OverallHealthScore { get; set; }
    public IEnumerable<IntegrationEndpoint> RecentlyFailedEndpoints { get; set; } = new List<IntegrationEndpoint>();
    public IEnumerable<IntegrationAlert> CriticalAlerts { get; set; } = new List<IntegrationAlert>();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
