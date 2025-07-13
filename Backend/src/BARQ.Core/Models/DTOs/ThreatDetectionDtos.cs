namespace BARQ.Core.Models.DTOs;

public class ThreatAssessmentDto
{
    public string ThreatLevel { get; set; } = string.Empty;
    public double RiskScore { get; set; }
    public string ThreatCategory { get; set; } = string.Empty;
    public IEnumerable<string> RiskFactors { get; set; } = new List<string>();
    public string RecommendedAction { get; set; } = string.Empty;
    public DateTime AssessedAt { get; set; }
    public string AssessmentMethod { get; set; } = string.Empty;
}

public class ThreatIndicatorDto
{
    public Guid Id { get; set; }
    public string IndicatorType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string ThreatType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string Source { get; set; } = string.Empty;
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
