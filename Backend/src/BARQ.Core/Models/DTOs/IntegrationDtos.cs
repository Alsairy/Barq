using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Integration request data transfer object for external API integration requests (important-comment)
/// </summary>
public class IntegrationRequest
{
    /// <summary>
    /// Unique identifier for the integration request
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Identifier of the target endpoint for the integration request
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// HTTP method for the integration request (GET, POST, PUT, DELETE, etc.)
    /// </summary>
    public string Method { get; set; } = "POST";
    /// <summary>
    /// URL path for the integration request endpoint
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// HTTP headers to be included in the integration request
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// Request body content for the integration request
    /// </summary>
    public string? Body { get; set; }
    /// <summary>
    /// Query parameters and other request parameters for the integration
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// Integration protocol type (REST, SOAP, GraphQL, etc.)
    /// </summary>
    public IntegrationProtocol Protocol { get; set; } = IntegrationProtocol.REST;
    /// <summary>
    /// Date and time when the integration request was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Tenant identifier for multi-tenant integration isolation
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Correlation identifier for tracking related integration requests
    /// </summary>
    public string? CorrelationId { get; set; }
    /// <summary>
    /// Timeout duration in seconds for the integration request
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// Integration response data transfer object containing the results of an external API integration request
/// </summary>
public class IntegrationResponse
{
    /// <summary>
    /// Unique identifier of the original integration request
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the integration request was successful
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// HTTP status code returned by the external API
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// Response body content from the external API
    /// </summary>
    public string? Body { get; set; }
    /// <summary>
    /// HTTP response headers returned by the external API
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// Error message if the integration request failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Date and time when the integration response was processed (important-comment)
    /// </summary>
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Processing time in milliseconds for the integration request
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    /// <summary>
    /// Identifier of the endpoint that processed the integration request
    /// </summary>
    public string? EndpointId { get; set; }
}

/// <summary>
/// Integration endpoint configuration for external API connections
/// </summary>
public class IntegrationEndpoint
{
    /// <summary>
    /// Unique identifier for the integration endpoint (important-comment)
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Display name of the integration endpoint
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description of the integration endpoint purpose
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Base URL for the external API endpoint
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    /// <summary>
    /// Protocol type used for integration communication
    /// </summary>
    public IntegrationProtocol Protocol { get; set; }
    /// <summary>
    /// Default HTTP headers to include in all requests
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    /// <summary>
    /// Type of authentication required for the endpoint
    /// </summary>
    public string? AuthenticationType { get; set; }
    /// <summary>
    /// Authentication configuration parameters
    /// </summary>
    public Dictionary<string, string> AuthenticationConfig { get; set; } = new();
    /// <summary>
    /// Indicates whether the endpoint is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// Request timeout duration in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    /// <summary>
    /// Number of retry attempts for failed requests
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    /// <summary>
    /// Delay between retry attempts
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    /// <summary>
    /// Tenant identifier for multi-tenant isolation (important-comment)
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Date and time when the endpoint was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Date and time of the last health check
    /// </summary>
    public DateTime? LastHealthCheck { get; set; }
    /// <summary>
    /// Current health status of the endpoint
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// Health status information for integration endpoints
/// </summary>
public class IntegrationHealthStatus
{
    /// <summary>
    /// Identifier of the endpoint being monitored
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the endpoint is currently healthy (important-comment)
    /// </summary>
    public bool IsHealthy { get; set; }
    /// <summary>
    /// Current status description of the endpoint
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Error message if the endpoint is unhealthy
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Date and time when the health check was performed
    /// </summary>
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Response time in milliseconds for the health check
    /// </summary>
    public long ResponseTimeMs { get; set; }
    /// <summary>
    /// Additional health check information and metrics
    /// </summary>
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();
}

/// <summary>
/// Integration request and response logging information
/// </summary>
public class IntegrationLog
{
    /// <summary>
    /// Unique identifier for the log entry
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Identifier of the associated integration request
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the endpoint that processed the request (important-comment)
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// HTTP method used for the integration request
    /// </summary>
    public string Method { get; set; } = string.Empty;
    /// <summary>
    /// URL path of the integration request
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// HTTP status code returned by the integration (important-comment)
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// Indicates whether the integration request was successful (important-comment)
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Processing time in milliseconds for the integration request (important-comment)
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    /// <summary>
    /// Error message if the integration request failed (important-comment)
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Date and time when the log entry was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Tenant identifier for multi-tenant log isolation (important-comment)
    /// </summary>
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
