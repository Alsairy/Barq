namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ThreatAssessmentDto
{
    /// <summary>
    /// </summary>
    public string ThreatLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double RiskScore { get; set; }
    
    /// <summary>
    /// </summary>
    public string ThreatCategory { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string RecommendedAction { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime AssessedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ThreatIndicatorDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string IndicatorType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Value { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ThreatType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// </summary>
public class BehavioralAnalysisDto
{
    /// <summary>
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double AnomalyScore { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsAnomalous { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> AnomalousPatterns { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public IEnumerable<UserActivityPatternDto> ActivityPatterns { get; set; } = new List<UserActivityPatternDto>();
    
    /// <summary>
    /// </summary>
    public DateTime AnalysisStartTime { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime AnalysisEndTime { get; set; }
    
    /// <summary>
    /// </summary>
    public string AnalysisMethod { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class UserActivityPatternDto
{
    /// <summary>
    /// </summary>
    public string ActivityType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Frequency { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageInterval { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime FirstOccurrence { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastOccurrence { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsNormal { get; set; }
    
    /// <summary>
    /// </summary>
    public double DeviationScore { get; set; }
}

/// <summary>
/// </summary>
public class GeolocationRiskDto
{
    /// <summary>
    /// </summary>
    public string IPAddress { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Region { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public double RiskScore { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public bool IsVpnDetected { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsProxyDetected { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsTorDetected { get; set; }
    
    /// <summary>
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime AssessedAt { get; set; }
}
