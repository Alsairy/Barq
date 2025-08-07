namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing threat assessment information (important-comment)
/// </summary>
public class ThreatAssessmentDto
{
    /// <summary>
    /// Gets or sets the threat level
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the risk score
    /// </summary>
    public double RiskScore { get; set; }
    
    /// <summary>
    /// Gets or sets the threat category
    /// </summary>
    public string ThreatCategory { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the list of risk factors
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    
    /// <summary>
    /// Gets or sets the recommended action
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the assessment date
    /// </summary>
    public DateTime AssessedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the assessment method
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object representing threat indicator information (important-comment)
/// </summary>
public class ThreatIndicatorDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the threat indicator (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the indicator type
    /// </summary>
    public string IndicatorType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the indicator value
    /// </summary>
    public string Value { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the threat type
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the severity level
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the expiration date
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the indicator is active (important-comment)
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the source of the threat indicator
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the threat indicator
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Data transfer object representing behavioral analysis information (important-comment)
/// </summary>
public class BehavioralAnalysisDto
{
    /// <summary>
    /// Gets or sets the user identifier
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the anomaly score
    /// </summary>
    public double AnomalyScore { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the behavior is anomalous (important-comment)
    /// </summary>
    public bool IsAnomalous { get; set; }
    
    /// <summary>
    /// Gets or sets the list of anomalous patterns
    /// </summary>
    public IEnumerable<string> AnomalousPatterns { get; set; } = new List<string>();
    
    /// <summary>
    /// Gets or sets the activity patterns
    /// </summary>
    public IEnumerable<UserActivityPatternDto> ActivityPatterns { get; set; } = new List<UserActivityPatternDto>();
    
    /// <summary>
    /// Gets or sets the analysis start time
    /// </summary>
    public DateTime AnalysisStartTime { get; set; }
    
    /// <summary>
    /// Gets or sets the analysis end time
    /// </summary>
    public DateTime AnalysisEndTime { get; set; }
    
    /// <summary>
    /// Gets or sets the analysis method
    /// </summary>
    public string AnalysisMethod { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object representing user activity pattern information (important-comment)
/// </summary>
public class UserActivityPatternDto
{
    /// <summary>
    /// Gets or sets the activity type
    /// </summary>
    public string ActivityType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the frequency of the activity
    /// </summary>
    public int Frequency { get; set; }
    
    /// <summary>
    /// Gets or sets the average interval between activities
    /// </summary>
    public TimeSpan AverageInterval { get; set; }
    
    /// <summary>
    /// Gets or sets the first occurrence date
    /// </summary>
    public DateTime FirstOccurrence { get; set; }
    
    /// <summary>
    /// Gets or sets the last occurrence date
    /// </summary>
    public DateTime LastOccurrence { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the pattern is normal (important-comment)
    /// </summary>
    public bool IsNormal { get; set; }
    
    /// <summary>
    /// Gets or sets the deviation score
    /// </summary>
    public double DeviationScore { get; set; }
}

/// <summary>
/// Represents geolocation risk assessment data for IP addresses
/// </summary>
public class GeolocationRiskDto
{
    /// <summary>
    /// Gets or sets the IP address being assessed
    /// </summary>
    public string IPAddress { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the country associated with the IP address
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the region or state associated with the IP address
    /// </summary>
    public string Region { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the city associated with the IP address
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the latitude coordinate of the IP address location
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Gets or sets the longitude coordinate of the IP address location
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Gets or sets the risk level classification (e.g., Low, Medium, High)
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the numerical risk score (0-100)
    /// </summary>
    public double RiskScore { get; set; }
    
    /// <summary>
    /// Gets or sets the list of risk factors contributing to the assessment (important-comment)
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    
    /// <summary>
    /// Gets or sets a value indicating whether VPN usage is detected (important-comment)
    /// </summary>
    public bool IsVpnDetected { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether proxy usage is detected (important-comment)
    /// </summary>
    public bool IsProxyDetected { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether Tor usage is detected (important-comment)
    /// </summary>
    public bool IsTorDetected { get; set; }
    
    /// <summary>
    /// Gets or sets the user identifier associated with this assessment (important-comment)
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the assessment was performed
    /// </summary>
    public DateTime AssessedAt { get; set; }
}
