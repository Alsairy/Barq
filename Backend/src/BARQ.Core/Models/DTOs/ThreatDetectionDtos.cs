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

public class BehavioralAnalysisDto
{
    public string UserId { get; set; } = string.Empty;
    public double AnomalyScore { get; set; }
    public bool IsAnomalous { get; set; }
    public IEnumerable<string> AnomalousPatterns { get; set; } = new List<string>();
    public IEnumerable<UserActivityPatternDto> ActivityPatterns { get; set; } = new List<UserActivityPatternDto>();
    public DateTime AnalysisStartTime { get; set; }
    public DateTime AnalysisEndTime { get; set; }
    public string AnalysisMethod { get; set; } = string.Empty;
}

public class UserActivityPatternDto
{
    public string ActivityType { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public TimeSpan AverageInterval { get; set; }
    public DateTime FirstOccurrence { get; set; }
    public DateTime LastOccurrence { get; set; }
    public bool IsNormal { get; set; }
    public double DeviationScore { get; set; }
}

public class GeolocationRiskDto
{
    public string IPAddress { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public double RiskScore { get; set; }
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    public bool IsVpnDetected { get; set; }
    public bool IsProxyDetected { get; set; }
    public bool IsTorDetected { get; set; }
    public string? UserId { get; set; }
    public DateTime AssessedAt { get; set; }
}
