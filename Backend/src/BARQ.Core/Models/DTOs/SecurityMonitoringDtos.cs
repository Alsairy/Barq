namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class SecurityEventDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; }
    
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
    public string? AdditionalData { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsResolved { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Resolution { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ResolvedBy { get; set; }
}

/// <summary>
/// </summary>
public class ThreatDetectionResultDto
{
    /// <summary>
    /// </summary>
    public bool IsThreat { get; set; }
    
    /// <summary>
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double ConfidenceScore { get; set; }
    
    /// <summary>
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> Indicators { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime DetectedAt { get; set; }
}

/// <summary>
/// </summary>
public class SecurityDashboardDto
{
    /// <summary>
    /// </summary>
    public int TotalEvents { get; set; }
    
    /// <summary>
    /// </summary>
    public int CriticalAlerts { get; set; }
    
    /// <summary>
    /// </summary>
    public int HighAlerts { get; set; }
    
    /// <summary>
    /// </summary>
    public int MediumAlerts { get; set; }
    
    /// <summary>
    /// </summary>
    public int LowAlerts { get; set; }
    
    /// <summary>
    /// </summary>
    public int ResolvedAlerts { get; set; }
    
    /// <summary>
    /// </summary>
    public int ActiveThreats { get; set; }
    
    /// <summary>
    /// </summary>
    public double ThreatScore { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<SecurityEventDto> RecentEvents { get; set; } = new List<SecurityEventDto>();
    
    /// <summary>
    /// </summary>
    public IEnumerable<ThreatTrendDto> ThreatTrends { get; set; } = new List<ThreatTrendDto>();
    
    /// <summary>
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// </summary>
public class SecurityAlertDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? IPAddress { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Resolution { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ResolvedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AdditionalContext { get; set; }
}

/// <summary>
/// </summary>
public class SecurityMetricsDto
{
    /// <summary>
    /// </summary>
    public int TotalSecurityEvents { get; set; }
    
    /// <summary>
    /// </summary>
    public int FailedLoginAttempts { get; set; }
    
    /// <summary>
    /// </summary>
    public int SuccessfulLogins { get; set; }
    
    /// <summary>
    /// </summary>
    public int BruteForceAttempts { get; set; }
    
    /// <summary>
    /// </summary>
    public int SuspiciousActivities { get; set; }
    
    /// <summary>
    /// </summary>
    public int DataAccessViolations { get; set; }
    
    /// <summary>
    /// </summary>
    public int PrivilegeEscalationAttempts { get; set; }
    
    /// <summary>
    /// </summary>
    public double AverageResponseTime { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime FromDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ToDate { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<SecurityTrendDto> Trends { get; set; } = new List<SecurityTrendDto>();
}

/// <summary>
/// </summary>
public class ThreatTrendDto
{
    /// <summary>
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class SecurityTrendDto
{
    /// <summary>
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// </summary>
    public string MetricType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Value { get; set; }
}
