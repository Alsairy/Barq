namespace BARQ.Core.Models.DTOs;

public class SiemEventDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? SessionId { get; set; }
    public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    public string? CorrelationId { get; set; }
    public string? TenantId { get; set; }
}

public class SiemCorrelationDto
{
    public string CorrelationId { get; set; } = string.Empty;
    public IEnumerable<SiemEventDto> RelatedEvents { get; set; } = new List<SiemEventDto>();
    public string CorrelationType { get; set; } = string.Empty;
    public double CorrelationScore { get; set; }
    public DateTime CorrelatedAt { get; set; }
    public string? Description { get; set; }
}

public class SiemHealthCheckDto
{
    public bool IsConnected { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastSuccessfulConnection { get; set; }
    public string? ErrorMessage { get; set; }
    public double ResponseTimeMs { get; set; }
    public string SiemVersion { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }
}

public class SiemConfigurationDto
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Format { get; set; } = "JSON";
    public bool IsEnabled { get; set; }
    public int BatchSize { get; set; } = 100;
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromMinutes(5);
    public IEnumerable<string> EnabledEventTypes { get; set; } = new List<string>();
    public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
    public int RetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
}

public class SiemAlertDto
{
    public string AlertId { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public IEnumerable<SiemEventDto> TriggerEvents { get; set; } = new List<SiemEventDto>();
    public string? Acknowledgment { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
}
