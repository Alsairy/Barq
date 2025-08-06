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

/// <summary>
/// Data transfer object for security alert information and management
/// </summary>
public class SecurityAlertDto
{
    /// <summary>
    /// Unique identifier for the security alert
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Type of security alert that was triggered
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed message describing the security alert
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the security alert
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the security alert was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Identifier of the user associated with the security alert
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// IP address from which the security alert originated (important-comment)
    /// </summary>
    public string? IPAddress { get; set; }
    /// <summary>
    /// Indicates whether the security alert is currently active
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Description of how the security alert was resolved
    /// </summary>
    public string? Resolution { get; set; }
    /// <summary>
    /// Date and time when the security alert was resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Identifier of the user who resolved the security alert
    /// </summary>
    public string? ResolvedBy { get; set; }
    /// <summary>
    /// Additional contextual information about the security alert
    /// </summary>
    public string? AdditionalContext { get; set; }
}

/// <summary>
/// Data transfer object for comprehensive security metrics and analytics
/// </summary>
public class SecurityMetricsDto
{
    /// <summary>
    /// Total number of security events recorded in the time period
    /// </summary>
    public int TotalSecurityEvents { get; set; }
    /// <summary>
    /// Number of failed login attempts detected
    /// </summary>
    public int FailedLoginAttempts { get; set; }
    /// <summary>
    /// Number of successful login events recorded
    /// </summary>
    public int SuccessfulLogins { get; set; }
    /// <summary>
    /// Number of brute force attack attempts detected
    /// </summary>
    public int BruteForceAttempts { get; set; }
    /// <summary>
    /// Number of suspicious activities flagged by the system
    /// </summary>
    public int SuspiciousActivities { get; set; }
    /// <summary>
    /// Number of unauthorized data access violations detected
    /// </summary>
    public int DataAccessViolations { get; set; }
    /// <summary>
    /// Number of privilege escalation attempts detected
    /// </summary>
    public int PrivilegeEscalationAttempts { get; set; }
    /// <summary>
    /// Average response time for security event processing in milliseconds
    /// </summary>
    public double AverageResponseTime { get; set; }
    /// <summary>
    /// Start date of the metrics reporting period
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date of the metrics reporting period
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Collection of security trend data for analytics visualization
    /// </summary>
    public IEnumerable<SecurityTrendDto> Trends { get; set; } = new List<SecurityTrendDto>();
}

/// <summary>
/// Data transfer object for threat trend analysis and visualization
/// </summary>
public class ThreatTrendDto
{
    /// <summary>
    /// Date of the threat trend data point
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Type of threat being tracked in the trend
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    /// <summary>
    /// Number of threats detected on this date
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Severity level of the threats in this trend
    /// </summary>
    public string Severity { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for security trend metrics and time-series data
/// </summary>
public class SecurityTrendDto
{
    /// <summary>
    /// Date of the security trend data point
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Type of security metric being tracked (important-comment)
    /// </summary>
    public string MetricType { get; set; } = string.Empty;
    /// <summary>
    /// Numerical value of the security metric for this date (important-comment)
    /// </summary>
    public int Value { get; set; }
}
