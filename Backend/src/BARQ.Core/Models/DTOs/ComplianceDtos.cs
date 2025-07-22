namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ComplianceAssessmentDto
{
    /// <summary>
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the date when the compliance assessment was conducted.
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// Gets or sets the current compliance status (e.g., Compliant, Non-Compliant, Partially Compliant).
    /// </summary>
    public string ComplianceStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the numerical compliance score (0-100).
    /// </summary>
    public decimal ComplianceScore { get; set; }

    /// <summary>
    /// Gets or sets the collection of identified compliance gaps or deficiencies.
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of recommended actions to address compliance issues.
    /// </summary>
    public IEnumerable<string> Recommendations { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the name of the person who conducted the assessment.
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the next compliance assessment is due.
    /// </summary>
    public DateTime NextAssessmentDue { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceReportDto
{
    /// <summary>
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// </summary>
    public DateTime ToDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ReportContent { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public IEnumerable<ComplianceMetricDto> Metrics { get; set; } = new List<ComplianceMetricDto>();

    /// <summary>
    /// </summary>
    public string GeneratedBy { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceMetricDto
{
    /// <summary>
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string MetricValue { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime MeasuredAt { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceViolationDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ViolationType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime DetectedAt { get; set; }

    /// <summary>
    /// </summary>
    public string DetectedBy { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Resolution { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ResolvedBy { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceConfigurationDto
{
    /// <summary>
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// </summary>
    public IEnumerable<string> EnabledRules { get; set; } = new List<string>();

    /// <summary>
    /// </summary>
    public IEnumerable<string> DisabledRules { get; set; } = new List<string>();

    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ComplianceAuditTrailDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// </summary>
    public string Changes { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string PerformedBy { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }
}
