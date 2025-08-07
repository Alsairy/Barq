namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for SIEM (Security Information and Event Management) event data
/// </summary>
public class SiemEventDto
{
    /// <summary>
    /// Unique identifier for the SIEM event
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Type of security event that occurred (important-comment)
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Source system or component that generated the event
    /// </summary>
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the security event (important-comment)
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the event occurred
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Detailed message describing the security event (important-comment)
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the user associated with the event (important-comment)
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// IP address from which the event originated
    /// </summary>
    public string? IPAddress { get; set; }
    /// <summary>
    /// User agent string from the client that triggered the event
    /// </summary>
    public string? UserAgent { get; set; }
    /// <summary>
    /// Session identifier associated with the event (important-comment)
    /// </summary>
    public string? SessionId { get; set; }
    /// <summary>
    /// Additional custom fields for extended event data
    /// </summary>
    public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    /// <summary>
    /// Correlation identifier for linking related events
    /// </summary>
    public string? CorrelationId { get; set; }
    /// <summary>
    /// Tenant identifier for multi-tenant event isolation (important-comment)
    /// </summary>
    public string? TenantId { get; set; }
}

/// <summary>
/// Data transfer object for SIEM event correlation analysis and grouping
/// </summary>
public class SiemCorrelationDto
{
    /// <summary>
    /// Unique identifier for the correlation group
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
    /// <summary>
    /// Collection of related security events in this correlation
    /// </summary>
    public IEnumerable<SiemEventDto> RelatedEvents { get; set; } = new List<SiemEventDto>();
    /// <summary>
    /// Type of correlation algorithm or pattern used
    /// </summary>
    public string CorrelationType { get; set; } = string.Empty;
    /// <summary>
    /// Numerical score indicating the strength of the correlation
    /// </summary>
    public double CorrelationScore { get; set; }
    /// <summary>
    /// Date and time when the correlation was established
    /// </summary>
    public DateTime CorrelatedAt { get; set; }
    /// <summary>
    /// Human-readable description of the correlation pattern
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Data transfer object for SIEM system health monitoring and connectivity status
/// </summary>
public class SiemHealthCheckDto
{
    /// <summary>
    /// Indicates whether the SIEM system is currently connected and accessible
    /// </summary>
    public bool IsConnected { get; set; }
    /// <summary>
    /// Current operational status of the SIEM system
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Date and time of the last successful connection to the SIEM system
    /// </summary>
    public DateTime LastSuccessfulConnection { get; set; }
    /// <summary>
    /// Error message if the health check failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// Response time in milliseconds for the health check request (important-comment)
    /// </summary>
    public double ResponseTimeMs { get; set; }
    /// <summary>
    /// Version information of the connected SIEM system
    /// </summary>
    public string SiemVersion { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when this health check was performed
    /// </summary>
    public DateTime CheckedAt { get; set; }
}

/// <summary>
/// Data transfer object for SIEM system integration configuration and settings
/// </summary>
public class SiemConfigurationDto
{
    /// <summary>
    /// SIEM system endpoint URL for API communication
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    /// <summary>
    /// API key for authenticating with the SIEM system
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    /// <summary>
    /// Data format for event transmission (JSON, XML, etc.)
    /// </summary>
    public string Format { get; set; } = "JSON";
    /// <summary>
    /// Indicates whether SIEM integration is currently enabled
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// Number of events to batch together for transmission
    /// </summary>
    public int BatchSize { get; set; } = 100;
    /// <summary>
    /// Time interval between batch transmissions to the SIEM system
    /// </summary>
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromMinutes(5);
    /// <summary>
    /// Collection of event types that are enabled for SIEM transmission
    /// </summary>
    public IEnumerable<string> EnabledEventTypes { get; set; } = new List<string>();
    /// <summary>
    /// Custom HTTP headers to include in SIEM API requests
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
    /// <summary>
    /// Number of retry attempts for failed SIEM transmissions (important-comment)
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    /// <summary>
    /// Delay between retry attempts for failed transmissions
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
}

/// <summary>
/// Data transfer object for SIEM security alerts and incident management
/// </summary>
public class SiemAlertDto
{
    /// <summary>
    /// Unique identifier for the SIEM alert
    /// </summary>
    public string AlertId { get; set; } = string.Empty;
    /// <summary>
    /// Type or category of the security alert
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    /// <summary>
    /// Brief title or summary of the alert
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the security alert and its implications (important-comment)
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the security alert (important-comment)
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the alert was created (important-comment)
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date and time when the alert was last updated (important-comment)
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Current status of the alert (open, investigating, resolved, etc.)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the user or team assigned to handle this alert
    /// </summary>
    public string? AssignedTo { get; set; }
    /// <summary>
    /// Collection of security events that triggered this alert
    /// </summary>
    public IEnumerable<SiemEventDto> TriggerEvents { get; set; } = new List<SiemEventDto>();
    /// <summary>
    /// Acknowledgment message or notes from the assigned handler
    /// </summary>
    public string? Acknowledgment { get; set; }
    /// <summary>
    /// Date and time when the alert was acknowledged (important-comment)
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }
    /// <summary>
    /// Identifier of the user who acknowledged the alert
    /// </summary>
    public string? AcknowledgedBy { get; set; }
}
