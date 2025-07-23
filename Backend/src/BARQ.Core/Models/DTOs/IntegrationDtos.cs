using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Represents a request to an external integration endpoint with all necessary configuration and data.
/// </summary>
public class IntegrationRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the integration request.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the identifier of the target integration endpoint.
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTTP method to use for the request (e.g., GET, POST, PUT, DELETE).
    /// </summary>
    public string Method { get; set; } = "POST";
    /// <summary>
    /// Gets or sets the URL path for the integration request.
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTTP headers to include with the request.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// Gets or sets the request body content as a string.
    /// </summary>
    public string? Body { get; set; }
    /// <summary>
    /// Gets or sets the query parameters and other request parameters.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// Gets or sets the integration protocol type (REST, SOAP, GraphQL, etc.).
    /// </summary>
    public IntegrationProtocol Protocol { get; set; } = IntegrationProtocol.REST;
    /// <summary>
    /// Gets or sets the timestamp when the request was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant isolation.
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Gets or sets the correlation identifier for request tracking across systems.
    /// </summary>
    public string? CorrelationId { get; set; }
    /// <summary>
    /// Gets or sets the timeout duration in seconds for the request.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// Represents the response received from an external integration endpoint.
/// </summary>
public class IntegrationResponse
{
    /// <summary>
    /// Gets or sets the identifier of the original request that generated this response.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the integration request was successful.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets the HTTP status code returned by the integration endpoint.
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Gets or sets the response body content as a string.
    /// </summary>
    public string? Body { get; set; }
    
    /// <summary>
    /// Gets or sets the HTTP response headers received from the endpoint.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the error message if the request failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when the response was processed.
    /// </summary>
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the total processing time in milliseconds.
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the endpoint that processed the request.
    /// </summary>
    public string? EndpointId { get; set; }
}

/// <summary>
/// Represents an external integration endpoint configuration with connection details and settings.
/// </summary>
public class IntegrationEndpoint
{
    /// <summary>
    /// Gets or sets the unique identifier for the integration endpoint.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Gets or sets the display name of the integration endpoint.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the detailed description of the integration endpoint's purpose and functionality.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the base URL for the integration endpoint.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the communication protocol used by the endpoint (REST, SOAP, GraphQL, etc.).
    /// </summary>
    public IntegrationProtocol Protocol { get; set; }
    
    /// <summary>
    /// Gets or sets the default HTTP headers to include with all requests to this endpoint.
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the type of authentication required by the endpoint (Bearer, Basic, ApiKey, etc.).
    /// </summary>
    public string? AuthenticationType { get; set; }
    
    /// <summary>
    /// Gets or sets the authentication configuration parameters specific to the endpoint.
    /// </summary>
    public Dictionary<string, string> AuthenticationConfig { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a value indicating whether the endpoint is currently active and available for use.
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// Gets or sets the timeout duration in seconds for requests to this endpoint.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    /// <summary>
    /// Gets or sets the number of retry attempts for failed requests.
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    /// <summary>
    /// Gets or sets the delay between retry attempts.
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant isolation.
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when the endpoint configuration was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the timestamp of the last health check performed on this endpoint.
    /// </summary>
    public DateTime? LastHealthCheck { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the endpoint is currently healthy and responsive.
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// Represents the health status information for an integration endpoint.
/// </summary>
public class IntegrationHealthStatus
{
    /// <summary>
    /// Gets or sets the identifier of the endpoint being monitored.
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the endpoint is currently healthy and responsive.
    /// </summary>
    public bool IsHealthy { get; set; }
    /// <summary>
    /// Gets or sets the current status description of the endpoint (e.g., "Online", "Offline", "Degraded").
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the error message if the endpoint is experiencing issues.
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when the health check was performed.
    /// </summary>
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the response time in milliseconds for the health check.
    /// </summary>
    public long ResponseTimeMs { get; set; }
    /// <summary>
    /// Gets or sets additional diagnostic information about the endpoint's health status.
    /// </summary>
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();
}

/// <summary>
/// Represents a log entry for integration requests and responses with detailed tracking information.
/// </summary>
public class IntegrationLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the log entry.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the identifier of the integration request that generated this log entry.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the identifier of the endpoint that processed the request.
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTTP method used for the integration request.
    /// </summary>
    public string Method { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the URL path that was accessed during the integration request.
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTTP status code returned by the integration endpoint.
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the integration request was successful.
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets the total processing time in milliseconds for the integration request.
    /// </summary>
    public long ProcessingTimeMs { get; set; }
    /// <summary>
    /// Gets or sets the error message if the integration request failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when the log entry was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant isolation.
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// Represents a message in the integration message queue system with processing metadata and status tracking.
/// </summary>
public class IntegrationMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the integration message.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the name of the queue where the message is stored.
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the type or category of the message.
    /// </summary>
    public string MessageType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the message content or payload.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the message headers containing metadata and routing information.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// Gets or sets the processing priority of the message.
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    /// <summary>
    /// Gets or sets the timestamp when the message was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the timestamp when the message was processed, if applicable.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    /// <summary>
    /// Gets or sets the current number of retry attempts for processing this message.
    /// </summary>
    public int RetryCount { get; set; } = 0;
    /// <summary>
    /// Gets or sets the maximum number of retry attempts allowed for this message.
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    /// <summary>
    /// Gets or sets the error message if message processing failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Gets or sets the current processing status of the message.
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Pending;
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant isolation.
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Gets or sets the correlation identifier for tracking related messages across systems.
    /// </summary>
    public string? CorrelationId { get; set; }
}

/// <summary>
/// Represents the result of a message transformation operation between different formats.
/// </summary>
public class MessageTransformationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the message transformation was successful.
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets the transformed message content in the target format.
    /// </summary>
    public string? TransformedContent { get; set; }
    /// <summary>
    /// Gets or sets the error message if the transformation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Gets or sets the original format of the message before transformation.
    /// </summary>
    public string SourceFormat { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the target format for the message transformation.
    /// </summary>
    public string TargetFormat { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp when the transformation was completed.
    /// </summary>
    public DateTime TransformedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents the current status and metrics of a message queue.
/// </summary>
public class QueueStatus
{
    /// <summary>
    /// Gets or sets the name of the message queue.
    /// </summary>
    public string QueueName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the number of messages waiting to be processed.
    /// </summary>
    public int PendingMessages { get; set; }
    /// <summary>
    /// Gets or sets the number of messages currently being processed.
    /// </summary>
    public int ProcessingMessages { get; set; }
    /// <summary>
    /// Gets or sets the number of messages that have been successfully processed.
    /// </summary>
    public int CompletedMessages { get; set; }
    /// <summary>
    /// Gets or sets the number of messages that failed processing.
    /// </summary>
    public int FailedMessages { get; set; }
    /// <summary>
    /// Gets or sets the timestamp when the queue status was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets a value indicating whether the queue is operating normally.
    /// </summary>
    public bool IsHealthy { get; set; } = true;
}

/// <summary>
/// Represents an event that occurred during integration processing for monitoring and auditing purposes.
/// </summary>
public class IntegrationEvent
{
    /// <summary>
    /// Gets or sets the unique identifier for the integration event.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the type or category of the integration event.
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the identifier of the endpoint associated with this event.
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of what occurred during the integration event.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the integration event (Info, Warning, Error, Critical).
    /// </summary>
    public IntegrationEventLevel Level { get; set; } = IntegrationEventLevel.Info;
    /// <summary>
    /// Gets or sets additional data and context information related to the event.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    /// <summary>
    /// Gets or sets the timestamp when the integration event occurred.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant isolation.
    /// </summary>
    public Guid TenantId { get; set; }
}

/// <summary>
/// Represents performance and usage metrics for integration endpoints over a specified time period.
/// </summary>
public class IntegrationMetrics
{
    /// <summary>
    /// Gets or sets the total number of integration requests processed.
    /// </summary>
    public int TotalRequests { get; set; }
    /// <summary>
    /// Gets or sets the number of integration requests that completed successfully.
    /// </summary>
    public int SuccessfulRequests { get; set; }
    /// <summary>
    /// Gets or sets the number of integration requests that failed.
    /// </summary>
    public int FailedRequests { get; set; }
    /// <summary>
    /// Gets or sets the success rate as a percentage (0.0 to 1.0).
    /// </summary>
    public double SuccessRate { get; set; }
    /// <summary>
    /// Gets or sets the average response time in milliseconds for all requests.
    /// </summary>
    public double AverageResponseTime { get; set; }
    /// <summary>
    /// Gets or sets the usage statistics for each integration endpoint.
    /// </summary>
    public Dictionary<string, int> EndpointUsage { get; set; } = new();
    /// <summary>
    /// Gets or sets the count of different error types encountered.
    /// </summary>
    public Dictionary<string, int> ErrorCounts { get; set; } = new();
    /// <summary>
    /// Gets or sets the start date of the metrics reporting period.
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// Gets or sets the end date of the metrics reporting period.
    /// </summary>
    public DateTime ToDate { get; set; }
}

/// <summary>
/// Represents an alert triggered by integration monitoring rules when specific conditions are met.
/// </summary>
public class IntegrationAlert
{
    /// <summary>
    /// Gets or sets the unique identifier for the integration alert.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the identifier of the alert rule that triggered this alert.
    /// </summary>
    public string RuleId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the title or summary of the alert.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the alert condition and impact.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the alert (Low, Medium, High, Critical).
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the endpoint associated with this alert.
    /// </summary>
    public string EndpointId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp when the alert was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Gets or sets the timestamp when the alert was resolved, if applicable.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the alert has been resolved.
    /// </summary>
    public bool IsResolved { get; set; } = false;
    /// <summary>
    /// Gets or sets additional contextual data related to the alert condition.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
}

/// <summary>
/// Represents a rule that defines conditions for triggering integration alerts.
/// </summary>
public class IntegrationAlertRule
{
    /// <summary>
    /// Gets or sets the unique identifier for the alert rule.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Gets or sets the display name of the alert rule.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of what the alert rule monitors.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the condition expression that triggers the alert when met.
    /// </summary>
    public string Condition { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level assigned to alerts triggered by this rule.
    /// </summary>
    public IntegrationAlertSeverity Severity { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the alert rule is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;
    /// <summary>
    /// Gets or sets the specific endpoint identifier this rule applies to, if any.
    /// </summary>
    public string? EndpointId { get; set; }
    /// <summary>
    /// Gets or sets the configuration parameters for the alert rule condition.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    /// <summary>
    /// Gets or sets the timestamp when the alert rule was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents a comprehensive health dashboard view of all integration endpoints and their status.
/// </summary>
public class IntegrationHealthDashboard
{
    /// <summary>
    /// Gets or sets the total number of configured integration endpoints.
    /// </summary>
    public int TotalEndpoints { get; set; }
    /// <summary>
    /// Gets or sets the number of endpoints that are currently healthy and responsive.
    /// </summary>
    public int HealthyEndpoints { get; set; }
    /// <summary>
    /// Gets or sets the number of endpoints that are currently experiencing issues.
    /// </summary>
    public int UnhealthyEndpoints { get; set; }
    /// <summary>
    /// Gets or sets the number of active alerts currently requiring attention.
    /// </summary>
    public int ActiveAlerts { get; set; }
    /// <summary>
    /// Gets or sets the overall health score as a percentage (0.0 to 1.0).
    /// </summary>
    public double OverallHealthScore { get; set; }
    /// <summary>
    /// Gets or sets the collection of endpoints that have recently experienced failures.
    /// </summary>
    public IEnumerable<IntegrationEndpoint> RecentlyFailedEndpoints { get; set; } = new List<IntegrationEndpoint>();
    /// <summary>
    /// Gets or sets the collection of critical alerts that require immediate attention.
    /// </summary>
    public IEnumerable<IntegrationAlert> CriticalAlerts { get; set; } = new List<IntegrationAlert>();
    /// <summary>
    /// Gets or sets the timestamp when the dashboard data was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
