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

/// <summary>
/// Integration message for queue-based communication and processing
/// </summary>
public class IntegrationMessage
{
    /// <summary>
    /// Unique identifier for the integration message (important-comment)
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Name of the queue where the message is processed
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// Type classification of the message content
    /// </summary>
    public string MessageType { get; set; } = string.Empty;
    /// <summary>
    /// Message payload content for processing
    /// </summary>
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// Message headers for routing and processing metadata
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// Processing priority level of the message
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    /// <summary>
    /// Date and time when the message was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Date and time when the message was processed
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    /// <summary>
    /// Number of retry attempts for failed message processing (important-comment)
    /// </summary>
    public int RetryCount { get; set; } = 0;
    /// <summary>
    /// Maximum number of retry attempts allowed
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    /// <summary>
    /// Error message if message processing failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Current processing status of the message
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Pending;
    /// <summary>
    /// Tenant identifier for multi-tenant message isolation (important-comment)
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Correlation identifier for tracking related messages (important-comment)
    /// </summary>
    public string? CorrelationId { get; set; }
}

/// <summary>
/// Result of message format transformation operations
/// </summary>
public class MessageTransformationResult
{
    /// <summary>
    /// Indicates whether the message transformation was successful
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Transformed message content in the target format
    /// </summary>
    public string? TransformedContent { get; set; }
    /// <summary>
    /// Error message if the transformation failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Original format of the source message
    /// </summary>
    public string SourceFormat { get; set; } = string.Empty;
    /// <summary>
    /// Target format for the transformed message
    /// </summary>
    public string TargetFormat { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the transformation was completed
    /// </summary>
    public DateTime TransformedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Status information for message processing queues
/// </summary>
public class QueueStatus
{
    /// <summary>
    /// Name of the message processing queue
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// Number of messages waiting to be processed
    /// </summary>
    public int PendingMessages { get; set; }
    /// <summary>
    /// Number of messages currently being processed
    /// </summary>
    public int ProcessingMessages { get; set; }
    /// <summary>
    /// Number of messages that have been successfully processed
    /// </summary>
    public int CompletedMessages { get; set; }
    /// <summary>
    /// Number of messages that failed processing
    /// </summary>
    public int FailedMessages { get; set; }
    /// <summary>
    /// Date and time when the queue status was last updated
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Indicates whether the queue is operating normally
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// Integration system event for monitoring and auditing
/// </summary>
public class IntegrationEvent
{
    /// <summary>
    /// Unique identifier for the integration event (important-comment)
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Type classification of the integration event
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the endpoint associated with the event
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the integration event
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the integration event
    /// </summary>
    public IntegrationEventLevel Level { get; set; } = IntegrationEventLevel.Info;
    /// <summary>
    /// Additional event data and context information
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    /// <summary>
    /// Date and time when the event was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Tenant identifier for multi-tenant event isolation (important-comment)
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// Performance and usage metrics for integration endpoints
/// </summary>
public class IntegrationMetrics
{
    /// <summary>
    /// Total number of integration requests processed
    /// </summary>
    public int TotalRequests { get; set; }
    /// <summary>
    /// Number of successful integration requests
    /// </summary>
    public int SuccessfulRequests { get; set; }
    /// <summary>
    /// Number of failed integration requests
    /// </summary>
    public int FailedRequests { get; set; }
    /// <summary>
    /// Success rate percentage for integration requests
    /// </summary>
    public double SuccessRate { get; set; }
    /// <summary>
    /// Average response time in milliseconds for integration requests
    /// </summary>
    public double AverageResponseTime { get; set; }
    /// <summary>
    /// Usage statistics by endpoint identifier
    /// </summary>
    public Dictionary<string, int> EndpointUsage { get; set; } = new();
    /// <summary>
    /// Error count statistics by error type
    /// </summary>
    public Dictionary<string, int> ErrorCounts { get; set; } = new();
    /// <summary>
    /// Start date for the metrics reporting period
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date for the metrics reporting period
    /// </summary>
    public DateTime ToDate { get; set; }
}

/// <summary>
/// Alert notification for integration system issues
/// </summary>
public class IntegrationAlert
{
    /// <summary>
    /// Unique identifier for the integration alert (important-comment)
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Identifier of the alert rule that triggered this alert
    /// </summary>
    public string RuleId { get; set; } = string.Empty;
    /// <summary>
    /// Title of the integration alert
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the integration alert
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the integration alert
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// Identifier of the endpoint associated with the alert
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the alert was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Date and time when the alert was resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Indicates whether the alert has been resolved
    /// </summary>
    public bool IsResolved { get; set; } = false;
    /// <summary>
    /// Additional alert data and context information
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
}

/// <summary>
/// Rule configuration for generating integration alerts
/// </summary>
public class IntegrationAlertRule
{
    /// <summary>
    /// Unique identifier for the alert rule
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Name of the alert rule
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description of the alert rule purpose
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Condition expression that triggers the alert
    /// </summary>
    public string Condition { get; set; } = string.Empty;
    /// <summary>
    /// Severity level for alerts generated by this rule
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// Indicates whether the alert rule is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// Specific endpoint identifier this rule applies to
    /// </summary>
    public string? EndpointId { get; set; }
    /// <summary>
    /// Configuration parameters for the alert rule
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// Date and time when the alert rule was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Dashboard view of integration system health and status
/// </summary>
public class IntegrationHealthDashboard
{
    /// <summary>
    /// Total number of configured integration endpoints
    /// </summary>
    public int TotalEndpoints { get; set; }
    /// <summary>
    /// Number of healthy integration endpoints
    /// </summary>
    public int HealthyEndpoints { get; set; }
    /// <summary>
    /// Number of unhealthy integration endpoints
    /// </summary>
    public int UnhealthyEndpoints { get; set; }
    /// <summary>
    /// Number of currently active alerts
    /// </summary>
    public int ActiveAlerts { get; set; }
    /// <summary>
    /// Overall health score percentage for the integration system
    /// </summary>
    public double OverallHealthScore { get; set; }
    /// <summary>
    /// List of endpoints that have recently failed
    /// </summary>
    public IEnumerable<IntegrationEndpoint> RecentlyFailedEndpoints { get; set; } = new List<IntegrationEndpoint>();
    /// <summary>
    /// List of critical alerts requiring immediate attention
    /// </summary>
    public IEnumerable<IntegrationAlert> CriticalAlerts { get; set; } = new List<IntegrationAlert>();
    /// <summary>
    /// Date and time when the dashboard was last updated
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
