namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class SiemEventDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the SIEM event.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the type of security event (e.g., Login, Logout, DataAccess, SecurityViolation).
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the source system or component that generated the event.
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the severity level of the event (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the timestamp when the event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Gets or sets the descriptive message about the security event.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the user associated with the event, if applicable.
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the IP address from which the event originated.
    /// </summary>
    public string? IPAddress { get; set; }
    
    /// <summary>
    /// Gets or sets the user agent string of the client that generated the event.
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// Gets or sets the session identifier associated with the event.
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// Gets or sets additional custom fields and metadata for the event.
    /// </summary>
    public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    
    /// <summary>
    /// Gets or sets the correlation identifier for linking related events.
    /// </summary>
    public string? CorrelationId { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant identifier for multi-tenant event isolation.
    /// </summary>
    public string? TenantId { get; set; }
}

/// <summary>
/// </summary>
public class SiemCorrelationDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the correlation group.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of related security events that are correlated together.
    /// </summary>
    public IEnumerable<SiemEventDto> RelatedEvents { get; set; } = new List<SiemEventDto>();
    
    /// <summary>
    /// Gets or sets the type of correlation pattern detected (e.g., TimeBasedPattern, UserBasedPattern, IPBasedPattern).
    /// </summary>
    public string CorrelationType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the confidence score of the correlation (0.0 to 1.0).
    /// </summary>
    public double CorrelationScore { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when the correlation was identified.
    /// </summary>
    public DateTime CorrelatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets an optional description of the correlation pattern or significance.
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// </summary>
public class SiemHealthCheckDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the SIEM system is currently connected and accessible.
    /// </summary>
    public bool IsConnected { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the SIEM connection (e.g., Connected, Disconnected, Error).
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the timestamp of the last successful connection to the SIEM system.
    /// </summary>
    public DateTime LastSuccessfulConnection { get; set; }
    
    /// <summary>
    /// Gets or sets the error message if the connection failed, otherwise null.
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Gets or sets the response time in milliseconds for the last health check.
    /// </summary>
    public double ResponseTimeMs { get; set; }
    
    /// <summary>
    /// Gets or sets the version of the SIEM system being connected to.
    /// </summary>
    public string SiemVersion { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the timestamp when this health check was performed.
    /// </summary>
    public DateTime CheckedAt { get; set; }
}

/// <summary>
/// Represents the configuration settings for integrating with a SIEM system.
/// </summary>
public class SiemConfigurationDto
{
    /// <summary>
    /// Gets or sets the SIEM system endpoint URL for sending events.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the API key for authenticating with the SIEM system.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the data format for sending events (e.g., JSON, XML, CEF).
    /// </summary>
    public string Format { get; set; } = "JSON";
    
    /// <summary>
    /// Gets or sets a value indicating whether SIEM integration is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Gets or sets the number of events to batch together before sending to SIEM.
    /// </summary>
    public int BatchSize { get; set; } = 100;
    
    /// <summary>
    /// Gets or sets the time interval between batch sends to the SIEM system.
    /// </summary>
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Gets or sets the collection of event types that should be sent to the SIEM system.
    /// </summary>
    public IEnumerable<string> EnabledEventTypes { get; set; } = new List<string>();
    
    /// <summary>
    /// Gets or sets custom HTTP headers to include when sending events to the SIEM system.
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
    
    /// <summary>
    /// Gets or sets the number of retry attempts for failed SIEM requests.
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    
    /// <summary>
    /// Gets or sets the delay between retry attempts for failed SIEM requests.
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
}

/// <summary>
/// Represents a security alert generated by or sent to a SIEM system.
/// </summary>
public class SiemAlertDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the security alert.
    /// </summary>
    public string AlertId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the type of security alert (e.g., IntrusionAttempt, DataBreach, AnomalousActivity).
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the title or summary of the security alert.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the detailed description of the security alert and its implications.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the severity level of the alert (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the timestamp when the alert was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when the alert was last updated, if applicable.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the alert (e.g., Open, InProgress, Resolved, Closed).
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the person or team assigned to handle the alert.
    /// </summary>
    public string? AssignedTo { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of security events that triggered this alert.
    /// </summary>
    public IEnumerable<SiemEventDto> TriggerEvents { get; set; } = new List<SiemEventDto>();
    
    /// <summary>
    /// Gets or sets the acknowledgment message or notes about the alert.
    /// </summary>
    public string? Acknowledgment { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when the alert was acknowledged.
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the person who acknowledged the alert.
    /// </summary>
    public string? AcknowledgedBy { get; set; }
}
