namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Represents a financial controls audit conducted for SOX compliance requirements.
/// </summary>
public class FinancialControlsAuditDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the financial controls audit.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the start date of the audit period.
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// Gets or sets the end date of the audit period.
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Gets or sets the specific control area being assessed (e.g., Financial Reporting, IT General Controls).
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the financial controls are effective and compliant.
    /// </summary>
    public bool IsEffective { get; set; }
    /// <summary>
    /// Gets or sets the collection of financial controls that were tested during the audit.
    /// </summary>
    public IEnumerable<string> TestedControls { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of deficiencies identified during the financial controls audit.
    /// </summary>
    public IEnumerable<string> Deficiencies { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the name of the auditor who conducted the financial controls audit.
    /// </summary>
    public string AuditorName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the financial controls audit was performed.
    /// </summary>
    public DateTime AuditDate { get; set; }
}

/// <summary>
/// Represents a change control log entry for tracking system changes under SOX compliance requirements.
/// </summary>
public class ChangeControlLogDto
{
    /// <summary>
    /// Gets or sets the type of change being implemented (e.g., Configuration, Code, Database).
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the system affected by the change.
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the change being implemented.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the person who requested the change.
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the person who approved the change request.
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the change was requested.
    /// </summary>
    public DateTime RequestDate { get; set; }
    /// <summary>
    /// Gets or sets the date when the change was approved.
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    /// <summary>
    /// Gets or sets the date when the change was implemented.
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    /// <summary>
    /// Gets or sets the business justification for implementing the change.
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the risk assessment analysis for the proposed change.
    /// </summary>
    public string RiskAssessment { get; set; } = string.Empty;
}

/// <summary>
/// Represents an audit record for change control processes under SOX compliance requirements.
/// </summary>
public class ChangeControlAuditDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the change control audit record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the type of change being implemented (e.g., Configuration, Code, Database).
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the system that was affected by the audited change.
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the change that was audited.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the person who requested the audited change.
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the person who approved the audited change.
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the audited change was requested.
    /// </summary>
    public DateTime RequestDate { get; set; }
    /// <summary>
    /// Gets or sets the date when the audited change was approved.
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    /// <summary>
    /// Gets or sets the date when the audited change was implemented.
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    /// <summary>
    /// Gets or sets the business justification for implementing the change.
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the risk assessment analysis for the proposed change.
    /// </summary>
    public string RiskAssessment { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the change control process met SOX compliance requirements.
    /// </summary>
    public bool ComplianceStatus { get; set; }
}

/// <summary>
/// </summary>
public class AccessControlsAuditDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the access controls audit.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the system being audited for access controls.
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the access controls audit was conducted.
    /// </summary>
    public DateTime AuditDate { get; set; }
    /// <summary>
    /// </summary>
    public string AuditorName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> UsersReviewed { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of access violations identified during the audit.
    /// </summary>
    public IEnumerable<string> AccessViolations { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of recommended actions to address access control deficiencies.
    /// </summary>
    public IEnumerable<string> RecommendedActions { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets a value indicating whether the overall access controls meet SOX compliance requirements.
    /// </summary>
    public bool OverallCompliance { get; set; }
}

/// <summary>
/// </summary>
public class DocumentationComplianceDto
{
    /// <summary>
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the required documentation exists for the process.
    /// </summary>
    public bool HasDocumentation { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the documentation is the current version.
    /// </summary>
    public bool IsCurrentVersion { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the documentation has the required approval signatures.
    /// </summary>
    public bool HasApprovalSignatures { get; set; }
    /// <summary>
    /// </summary>
    public DateTime LastReviewDate { get; set; }
    /// <summary>
    /// Gets or sets the collection of documents that are missing for full compliance.
    /// </summary>
    public IEnumerable<string> MissingDocuments { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the overall compliance status of the process documentation.
    /// </summary>
    public string ComplianceStatus { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class InternalControlsAssessmentDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the internal controls assessment.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the specific control area being assessed (e.g., Financial Reporting, IT General Controls).
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the internal control being assessed.
    /// </summary>
    public string ControlDescription { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the person responsible for the internal control.
    /// </summary>
    public string ControlOwner { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ControlFrequency { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the internal control is operating effectively.
    /// </summary>
    public bool IsEffective { get; set; }
    /// <summary>
    /// </summary>
    public string TestingProcedure { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime LastTested { get; set; }
    /// <summary>
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
}

public class AuditTrailComplianceDto
{
    public string SystemName { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCompliant { get; set; }
    public int TotalTransactions { get; set; }
    public int AuditedTransactions { get; set; }
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    public string CompliancePercentage { get; set; } = string.Empty;
}

public class InternalControlDeficiencyDto
{
    public string ControlName { get; set; } = string.Empty;
    public string DeficiencyType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string RootCause { get; set; } = string.Empty;
    public string ImpactAssessment { get; set; } = string.Empty;
    public DateTime IdentifiedDate { get; set; }
    public string IdentifiedBy { get; set; } = string.Empty;
}

public class DeficiencyReportDto
{
    public Guid Id { get; set; }
    public string ControlName { get; set; } = string.Empty;
    public string DeficiencyType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ReportedDate { get; set; }
    public string ReportedBy { get; set; } = string.Empty;
    public DateTime? RemediationDate { get; set; }
    public string RemediationPlan { get; set; } = string.Empty;
}

public class ComplianceTestingDto
{
    public Guid Id { get; set; }
    public string ControlName { get; set; } = string.Empty;
    public string TestProcedure { get; set; } = string.Empty;
    public DateTime TestDate { get; set; }
    public string TestPerformedBy { get; set; } = string.Empty;
    public string TestResults { get; set; } = string.Empty;
    public bool TestPassed { get; set; }
    public IEnumerable<string> Exceptions { get; set; } = new List<string>();
    public string RecommendedActions { get; set; } = string.Empty;
}

public class ManagementAssertionDto
{
    public string ControlObjective { get; set; } = string.Empty;
    public string AssertionStatement { get; set; } = string.Empty;
    public string ManagementResponse { get; set; } = string.Empty;
    public DateTime AssertionDate { get; set; }
    public string AssertedBy { get; set; } = string.Empty;
    public IEnumerable<string> SupportingEvidence { get; set; } = new List<string>();
    public bool IsEffective { get; set; }
}

public class RemediationRequestDto
{
    public string DeficiencyId { get; set; } = string.Empty;
    public string RemediationPlan { get; set; } = string.Empty;
    public DateTime TargetCompletionDate { get; set; }
    public string ResponsibleParty { get; set; } = string.Empty;
    public IEnumerable<string> RequiredResources { get; set; } = new List<string>();
    public string BusinessImpact { get; set; } = string.Empty;
}

public class RemediationPlanDto
{
    public Guid Id { get; set; }
    public string DeficiencyId { get; set; } = string.Empty;
    public string RemediationPlan { get; set; } = string.Empty;
    public DateTime TargetCompletionDate { get; set; }
    public string ResponsibleParty { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string CompletionNotes { get; set; } = string.Empty;
}
