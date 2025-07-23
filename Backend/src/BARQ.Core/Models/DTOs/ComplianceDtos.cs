namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ComplianceAssessmentDto
{
    /// <summary>
    /// Gets or sets the regulatory framework being assessed (e.g., GDPR, HIPAA, SOX).
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
    /// Gets or sets the regulatory framework covered by this report (e.g., GDPR, HIPAA, SOX).
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date of the reporting period.
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the reporting period.
    /// </summary>
    public DateTime ToDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the type of compliance report (e.g., Summary, Detailed, Executive).
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the main content and findings of the compliance report.
    /// </summary>
    public string ReportContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of compliance metrics and measurements included in the report.
    /// </summary>
    public IEnumerable<ComplianceMetricDto> Metrics { get; set; } = new List<ComplianceMetricDto>();

    /// <summary>
    /// Gets or sets the name of the person or system that generated the report.
    /// </summary>
    public string GeneratedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the report was generated.
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceMetricDto
{
    /// <summary>
    /// Gets or sets the name of the compliance metric being measured.
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the measured value of the compliance metric.
    /// </summary>
    public string MetricValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of metric (e.g., Percentage, Count, Boolean).
    /// </summary>
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the metric was measured.
    /// </summary>
    public DateTime MeasuredAt { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceViolationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the compliance violation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the regulatory framework that was violated (e.g., GDPR, HIPAA, SOX).
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type or category of the compliance violation.
    /// </summary>
    public string ViolationType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the compliance violation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity level of the violation (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the violation was detected.
    /// </summary>
    public DateTime DetectedAt { get; set; }

    /// <summary>
    /// Gets or sets the name of the person or system that detected the violation.
    /// </summary>
    public string DetectedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the violation (e.g., Open, In Progress, Resolved).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of how the violation was resolved.
    /// </summary>
    public string Resolution { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the person who resolved the violation.
    /// </summary>
    public string ResolvedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the violation was resolved.
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
    /// Gets or sets the regulatory framework being configured (e.g., GDPR, HIPAA, SOX).
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether compliance monitoring is enabled for this framework.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the configuration settings and parameters for the compliance framework.
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the collection of compliance rules that are currently enabled.
    /// </summary>
    public IEnumerable<string> EnabledRules { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of compliance rules that are currently disabled.
    /// </summary>
    public IEnumerable<string> DisabledRules { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the date and time when the configuration was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the name of the person who last updated the configuration.
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ComplianceAuditTrailDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the audit trail entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the regulatory framework associated with this audit entry (e.g., GDPR, HIPAA, SOX).
    /// </summary>
    public string Framework { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action that was performed (e.g., Create, Update, Delete, Access).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of entity that was affected by the action.
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the entity that was affected.
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the detailed description of the changes that were made.
    /// </summary>
    public string Changes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the user who performed the action.
    /// </summary>
    public string PerformedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the action was performed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the IP address from which the action was performed.
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user agent string of the client that performed the action.
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the organization being assessed.
    /// </summary>
    public Guid? OrganizationId { get; set; }
}
