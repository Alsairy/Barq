namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class SecurityEventDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the security event.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the type of security event (e.g., Login Attempt, Data Access, Permission Change).
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the security event.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the security event (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the security event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the user associated with the security event.
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the security event originated.
    /// </summary>
    public string? IPAddress { get; set; }
    /// <summary>
    /// Gets or sets the user agent string of the client that triggered the security event.
    /// </summary>
    public string? UserAgent { get; set; }
    /// <summary>
    /// Gets or sets the session identifier associated with the security event.
    /// </summary>
    public string? SessionId { get; set; }
    /// <summary>
    /// Gets or sets additional contextual data related to the security event.
    /// </summary>
    public string? AdditionalData { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the security event has been resolved.
    /// </summary>
    public bool IsResolved { get; set; }
    /// <summary>
    /// Gets or sets the description of how the security event was resolved.
    /// </summary>
    public string? Resolution { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the security event was resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the user who resolved the security event.
    /// </summary>
    public string? ResolvedBy { get; set; }
}

/// <summary>
/// </summary>
public class ThreatDetectionResultDto
{
    /// <summary>
    /// Gets or sets a value indicating whether a threat was detected.
    /// </summary>
    public bool IsThreat { get; set; }
    /// <summary>
    /// Gets or sets the threat level assigned to the detection (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the confidence score of the threat detection (0.0 to 1.0).
    /// </summary>
    public double ConfidenceScore { get; set; }
    /// <summary>
    /// Gets or sets the type of threat detected (e.g., Malware, Phishing, Data Breach).
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the detected threat.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of indicators that led to the threat detection.
    /// </summary>
    public IEnumerable<string> Indicators { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the recommended action to address the detected threat.
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the threat was detected.
    /// </summary>
    public DateTime DetectedAt { get; set; }
}

/// <summary>
/// </summary>
public class SecurityDashboardDto
{
    /// <summary>
    /// Gets or sets the total number of security events recorded.
    /// </summary>
    public int TotalEvents { get; set; }
    /// <summary>
    /// Gets or sets the number of critical security alerts.
    /// </summary>
    public int CriticalAlerts { get; set; }
    /// <summary>
    /// Gets or sets the number of high priority security alerts.
    /// </summary>
    public int HighAlerts { get; set; }
    /// <summary>
    /// Gets or sets the number of medium priority security alerts.
    /// </summary>
    public int MediumAlerts { get; set; }
    /// <summary>
    /// Gets or sets the number of low priority security alerts.
    /// </summary>
    public int LowAlerts { get; set; }
    /// <summary>
    /// Gets or sets the number of resolved security alerts.
    /// </summary>
    public int ResolvedAlerts { get; set; }
    /// <summary>
    /// Gets or sets the number of currently active threats.
    /// </summary>
    public int ActiveThreats { get; set; }
    /// <summary>
    /// Gets or sets the overall threat score for the organization (0.0 to 1.0).
    /// </summary>
    public double ThreatScore { get; set; }
    /// <summary>
    /// Gets or sets the collection of recent security events.
    /// </summary>
    public IEnumerable<SecurityEventDto> RecentEvents { get; set; } = new List<SecurityEventDto>();
    /// <summary>
    /// Gets or sets the collection of threat trends over time.
    /// </summary>
    public IEnumerable<ThreatTrendDto> ThreatTrends { get; set; } = new List<ThreatTrendDto>();
    /// <summary>
    /// Gets or sets the date and time when the dashboard data was generated.
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// </summary>
public class SecurityAlertDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the security alert.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the type of security alert (e.g., Intrusion Attempt, Data Breach, Malware Detection).
    /// </summary>
    public string AlertType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the alert message describing the security incident.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the security alert (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the security alert was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the user associated with the security alert.
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the security alert originated.
    /// </summary>
    public string? IPAddress { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the security alert is currently active.
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Gets or sets the description of how the security alert was resolved.
    /// </summary>
    public string? Resolution { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the security alert was resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the user who resolved the security alert.
    /// </summary>
    public string? ResolvedBy { get; set; }
    /// <summary>
    /// Gets or sets additional contextual information related to the security alert.
    /// </summary>
    public string? AdditionalContext { get; set; }
}

/// <summary>
/// </summary>
public class SecurityMetricsDto
{
    /// <summary>
    /// Gets or sets the total number of security events recorded.
    /// </summary>
    public int TotalSecurityEvents { get; set; }
    /// <summary>
    /// Gets or sets the number of failed login attempts.
    /// </summary>
    public int FailedLoginAttempts { get; set; }
    /// <summary>
    /// Gets or sets the number of successful login attempts.
    /// </summary>
    public int SuccessfulLogins { get; set; }
    /// <summary>
    /// Gets or sets the number of brute force attack attempts detected.
    /// </summary>
    public int BruteForceAttempts { get; set; }
    /// <summary>
    /// Gets or sets the number of suspicious activities detected.
    /// </summary>
    public int SuspiciousActivities { get; set; }
    /// <summary>
    /// Gets or sets the number of data access violations detected.
    /// </summary>
    public int DataAccessViolations { get; set; }
    /// <summary>
    /// Gets or sets the number of privilege escalation attempts detected.
    /// </summary>
    public int PrivilegeEscalationAttempts { get; set; }
    /// <summary>
    /// Gets or sets the average response time for security incident resolution in minutes.
    /// </summary>
    public double AverageResponseTime { get; set; }
    /// <summary>
    /// Gets or sets the start date of the metrics reporting period.
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// Gets or sets the end date of the metrics reporting period.
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Gets or sets the collection of security trends over time.
    /// </summary>
    public IEnumerable<SecurityTrendDto> Trends { get; set; } = new List<SecurityTrendDto>();
}

/// <summary>
/// </summary>
public class ThreatTrendDto
{
    /// <summary>
    /// Gets or sets the date for which the threat trend data is recorded.
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Gets or sets the type of threat being tracked (e.g., Malware, Phishing, Data Breach).
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the number of threats of this type detected on the specified date.
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Gets or sets the severity level of the threats (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class SecurityTrendDto
{
    /// <summary>
    /// Gets or sets the date for which the security trend data is recorded.
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Gets or sets the type of security metric being tracked (e.g., Login Attempts, Data Access, Alerts).
    /// </summary>
    public string MetricType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the numerical value of the security metric for the specified date.
    /// </summary>
    public int Value { get; set; }
}
