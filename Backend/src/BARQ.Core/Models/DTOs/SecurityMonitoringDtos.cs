namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for security event information and monitoring details
/// </summary>
public class SecurityEventDto
{
    /// <summary>
    /// Unique identifier for the security event
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Type of security event that occurred
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the security event (important-comment)
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the security event
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the security event occurred
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Identifier of the user associated with the security event
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// IP address from which the security event originated
    /// </summary>
    public string? IPAddress { get; set; }
    /// <summary>
    /// User agent string of the client that triggered the security event (important-comment)
    /// </summary>
    public string? UserAgent { get; set; }
    /// <summary>
    /// Session identifier associated with the security event
    /// </summary>
    public string? SessionId { get; set; }
    /// <summary>
    /// Additional contextual data related to the security event
    /// </summary>
    public string? AdditionalData { get; set; }
    /// <summary>
    /// Indicates whether the security event has been resolved
    /// </summary>
    public bool IsResolved { get; set; }
    /// <summary>
    /// Description of how the security event was resolved
    /// </summary>
    public string? Resolution { get; set; }
    /// <summary>
    /// Date and time when the security event was resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Identifier of the user who resolved the security event
    /// </summary>
    public string? ResolvedBy { get; set; }
}

/// <summary>
/// Data transfer object for threat detection analysis results and recommendations
/// </summary>
public class ThreatDetectionResultDto
{
    /// <summary>
    /// Indicates whether a threat was detected in the analysis
    /// </summary>
    public bool IsThreat { get; set; }
    /// <summary>
    /// Severity level of the detected threat
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    /// <summary>
    /// Confidence score of the threat detection algorithm
    /// </summary>
    public double ConfidenceScore { get; set; }
    /// <summary>
    /// Type or category of the detected threat
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the detected threat
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// List of indicators that led to the threat detection
    /// </summary>
    public IEnumerable<string> Indicators { get; set; } = new List<string>();
    /// <summary>
    /// Recommended action to take in response to the threat
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the threat was detected
    /// </summary>
    public DateTime DetectedAt { get; set; }
}

/// <summary>
/// Data transfer object for security dashboard metrics and summary information
/// </summary>
public class SecurityDashboardDto
{
    /// <summary>
    /// Total number of security events recorded
    /// </summary>
    public int TotalEvents { get; set; }
    /// <summary>
    /// Number of critical severity security alerts
    /// </summary>
    public int CriticalAlerts { get; set; }
    /// <summary>
    /// Number of high severity security alerts
    /// </summary>
    public int HighAlerts { get; set; }
    /// <summary>
    /// Number of medium severity security alerts
    /// </summary>
    public int MediumAlerts { get; set; }
    /// <summary>
    /// Number of low severity security alerts
    /// </summary>
    public int LowAlerts { get; set; }
    /// <summary>
    /// Number of security alerts that have been resolved
    /// </summary>
    public int ResolvedAlerts { get; set; }
    /// <summary>
    /// Number of currently active security threats
    /// </summary>
    public int ActiveThreats { get; set; }
    /// <summary>
    /// Overall threat score calculated from security metrics
    /// </summary>
    public double ThreatScore { get; set; }
    /// <summary>
    /// Collection of recent security events for dashboard display
    /// </summary>
    public IEnumerable<SecurityEventDto> RecentEvents { get; set; } = new List<SecurityEventDto>();
    /// <summary>
    /// Collection of threat trend data for analytics visualization
    /// </summary>
    public IEnumerable<ThreatTrendDto> ThreatTrends { get; set; } = new List<ThreatTrendDto>();
    /// <summary>
    /// Date and time when the dashboard data was generated
    /// </summary>
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
