namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class SiemEventDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? IPAddress { get; set; }
    
    /// <summary>
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    
    /// <summary>
    /// </summary>
    public string? CorrelationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TenantId { get; set; }
}

/// <summary>
/// </summary>
public class SiemCorrelationDto
{
    /// <summary>
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<SiemEventDto> RelatedEvents { get; set; } = new List<SiemEventDto>();
    
    /// <summary>
    /// </summary>
    public string CorrelationType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double CorrelationScore { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CorrelatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// </summary>
public class SiemHealthCheckDto
{
    /// <summary>
    /// </summary>
    public bool IsConnected { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime LastSuccessfulConnection { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public double ResponseTimeMs { get; set; }
    
    /// <summary>
    /// </summary>
    public string SiemVersion { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CheckedAt { get; set; }
}

/// <summary>
/// </summary>
public class SiemConfigurationDto
{
    /// <summary>
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Format { get; set; } = "JSON";
    
    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// </summary>
    public int BatchSize { get; set; } = 100;
    
    /// <summary>
    /// </summary>
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> EnabledEventTypes { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
    
    /// <summary>
    /// </summary>
    public int RetryAttempts { get; set; } = 3;
    
    /// <summary>
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
}

/// <summary>
/// </summary>
public class SiemAlertDto
{
    /// <summary>
    /// </summary>
    public string AlertId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? AssignedTo { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<SiemEventDto> TriggerEvents { get; set; } = new List<SiemEventDto>();
    
    /// <summary>
    /// </summary>
    public string? Acknowledgment { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AcknowledgedBy { get; set; }
}
