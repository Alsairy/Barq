namespace BARQ.Core.Models.DTOs;

public class SecurityEventDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? UserId { get; set; }
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? SessionId { get; set; }
    public string? AdditionalData { get; set; }
    public bool IsResolved { get; set; }
    public string? Resolution { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedBy { get; set; }
}

public class ThreatDetectionResultDto
{
    public bool IsThreat { get; set; }
    public string ThreatLevel { get; set; } = string.Empty;
    public double ConfidenceScore { get; set; }
    public string ThreatType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<string> Indicators { get; set; } = new List<string>();
    public string RecommendedAction { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
}

public class SecurityDashboardDto
{
    public int TotalEvents { get; set; }
    public int CriticalAlerts { get; set; }
    public int HighAlerts { get; set; }
    public int MediumAlerts { get; set; }
    public int LowAlerts { get; set; }
    public int ResolvedAlerts { get; set; }
    public int ActiveThreats { get; set; }
    public double ThreatScore { get; set; }
    public IEnumerable<SecurityEventDto> RecentEvents { get; set; } = new List<SecurityEventDto>();
    public IEnumerable<ThreatTrendDto> ThreatTrends { get; set; } = new List<ThreatTrendDto>();
    public DateTime GeneratedAt { get; set; }
}

public class SecurityAlertDto
{
    public Guid Id { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UserId { get; set; }
    public string? IPAddress { get; set; }
    public bool IsActive { get; set; }
    public string? Resolution { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedBy { get; set; }
    public string? AdditionalContext { get; set; }
}

public class SecurityMetricsDto
{
    public int TotalSecurityEvents { get; set; }
    public int FailedLoginAttempts { get; set; }
    public int SuccessfulLogins { get; set; }
    public int BruteForceAttempts { get; set; }
    public int SuspiciousActivities { get; set; }
    public int DataAccessViolations { get; set; }
    public int PrivilegeEscalationAttempts { get; set; }
    public double AverageResponseTime { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public IEnumerable<SecurityTrendDto> Trends { get; set; } = new List<SecurityTrendDto>();
}

public class ThreatTrendDto
{
    public DateTime Date { get; set; }
    public string ThreatType { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Severity { get; set; } = string.Empty;
}

public class SecurityTrendDto
{
    public DateTime Date { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public int Value { get; set; }
}
