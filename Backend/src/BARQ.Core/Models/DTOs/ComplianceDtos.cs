namespace BARQ.Core.Models.DTOs;

public class ComplianceAssessmentDto
{
    public string Framework { get; set; } = string.Empty;
    public Guid? OrganizationId { get; set; }
    public DateTime AssessmentDate { get; set; }
    public string ComplianceStatus { get; set; } = string.Empty;
    public decimal ComplianceScore { get; set; }
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    public IEnumerable<string> Recommendations { get; set; } = new List<string>();
    public string AssessedBy { get; set; } = string.Empty;
    public DateTime NextAssessmentDue { get; set; }
}

public class ComplianceReportDto
{
    public string Framework { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? OrganizationId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public string ReportContent { get; set; } = string.Empty;
    public IEnumerable<ComplianceMetricDto> Metrics { get; set; } = new List<ComplianceMetricDto>();
    public string GeneratedBy { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class ComplianceMetricDto
{
    public string MetricName { get; set; } = string.Empty;
    public string MetricValue { get; set; } = string.Empty;
    public string MetricType { get; set; } = string.Empty;
    public DateTime MeasuredAt { get; set; }
}

public class ComplianceViolationDto
{
    public Guid Id { get; set; }
    public string Framework { get; set; } = string.Empty;
    public string ViolationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
    public string DetectedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public string ResolvedBy { get; set; } = string.Empty;
    public DateTime? ResolvedAt { get; set; }
    public Guid? OrganizationId { get; set; }
}

public class ComplianceConfigurationDto
{
    public string Framework { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    public IEnumerable<string> EnabledRules { get; set; } = new List<string>();
    public IEnumerable<string> DisabledRules { get; set; } = new List<string>();
    public DateTime LastUpdated { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}

public class ComplianceAuditTrailDto
{
    public Guid Id { get; set; }
    public string Framework { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string Changes { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public Guid? OrganizationId { get; set; }
}
