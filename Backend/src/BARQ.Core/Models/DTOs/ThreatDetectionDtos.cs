namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ThreatAssessmentDto
{
    /// <summary>
    /// Gets or sets the threat level assigned to the assessment (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public double RiskScore { get; set; }
    /// <summary>
    /// Gets or sets the category of the threat being assessed (e.g., Malware, Phishing, Data Breach).
    /// </summary>
    public string ThreatCategory { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of risk factors that contributed to the threat assessment.
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the recommended action to address the identified threat.
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the threat assessment was conducted.
    /// </summary>
    public DateTime AssessedAt { get; set; }
    /// <summary>
    /// Gets or sets the method used to conduct the threat assessment (e.g., Automated, Manual, Hybrid).
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ThreatIndicatorDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the threat indicator.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the type of threat indicator (e.g., IP Address, Domain, File Hash, URL).
    /// </summary>
    public string IndicatorType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the value of the threat indicator.
    /// </summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the type of threat associated with this indicator (e.g., Malware, Phishing).
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the threat indicator (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the threat indicator was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the threat indicator expires.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the threat indicator is currently active.
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Gets or sets the source of the threat indicator intelligence.
    /// </summary>
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the threat indicator.
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// </summary>
public class BehavioralAnalysisDto
{
    /// <summary>
    /// Gets or sets the identifier of the user being analyzed.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the anomaly score calculated for the user's behavior (0.0 to 1.0).
    /// </summary>
    public double AnomalyScore { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the user's behavior is considered anomalous.
    /// </summary>
    public bool IsAnomalous { get; set; }
    /// <summary>
    /// Gets or sets the collection of anomalous patterns identified in the user's behavior.
    /// </summary>
    public IEnumerable<string> AnomalousPatterns { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of activity patterns observed for the user.
    /// </summary>
    public IEnumerable<UserActivityPatternDto> ActivityPatterns { get; set; } = new List<UserActivityPatternDto>();
    /// <summary>
    /// Gets or sets the start time of the behavioral analysis period.
    /// </summary>
    public DateTime AnalysisStartTime { get; set; }
    /// <summary>
    /// Gets or sets the end time of the behavioral analysis period.
    /// </summary>
    public DateTime AnalysisEndTime { get; set; }
    /// <summary>
    /// Gets or sets the method used to conduct the behavioral analysis (e.g., Machine Learning, Statistical).
    /// </summary>
    public string AnalysisMethod { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class UserActivityPatternDto
{
    /// <summary>
    /// Gets or sets the type of activity performed by the user.
    /// </summary>
    public string ActivityType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the frequency of the activity occurrence.
    /// </summary>
    public int Frequency { get; set; }
    /// <summary>
    /// Gets or sets the average time interval between activity occurrences.
    /// </summary>
    public TimeSpan AverageInterval { get; set; }
    /// <summary>
    /// Gets or sets the date and time of the first occurrence of this activity pattern.
    /// </summary>
    public DateTime FirstOccurrence { get; set; }
    /// <summary>
    /// Gets or sets the date and time of the last occurrence of this activity pattern.
    /// </summary>
    public DateTime LastOccurrence { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this activity pattern is considered normal.
    /// </summary>
    public bool IsNormal { get; set; }
    /// <summary>
    /// Gets or sets the deviation score indicating how much this pattern deviates from normal behavior.
    /// </summary>
    public double DeviationScore { get; set; }
}

/// <summary>
/// </summary>
public class GeolocationRiskDto
{
    /// <summary>
    /// Gets or sets the IP address being assessed for geolocation risk.
    /// </summary>
    public string IPAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the country from which the activity originated.
    /// </summary>
    public string Country { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the region from which the activity originated.
    /// </summary>
    public string Region { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the city from which the activity originated.
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the latitude coordinate of the geographic location.
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// Gets or sets the longitude coordinate of the geographic location.
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// Gets or sets the risk level assigned to the geographic location (e.g., Low, Medium, High).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the numerical risk score calculated for the geographic location (0.0 to 1.0).
    /// </summary>
    public double RiskScore { get; set; }
    /// <summary>
    /// Gets or sets the collection of risk factors associated with the geographic location.
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets a value indicating whether VPN usage was detected from this location.
    /// </summary>
    public bool IsVpnDetected { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether proxy usage was detected from this location.
    /// </summary>
    public bool IsProxyDetected { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether Tor network usage was detected from this location.
    /// </summary>
    public bool IsTorDetected { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the user associated with this geolocation risk assessment.
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the geolocation risk assessment was conducted.
    /// </summary>
    public DateTime AssessedAt { get; set; }
}
