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

/// <summary>
/// Represents an audit trail compliance assessment for SOX requirements.
/// </summary>
public class AuditTrailComplianceDto
{
    /// <summary>
    /// Gets or sets the name of the system being assessed for audit trail compliance.
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the start date of the audit trail compliance assessment period.
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// Gets or sets the end date of the audit trail compliance assessment period.
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the audit trail meets SOX compliance requirements.
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Gets or sets the total number of transactions processed during the assessment period.
    /// </summary>
    public int TotalTransactions { get; set; }
    /// <summary>
    /// Gets or sets the number of transactions that were audited for compliance.
    /// </summary>
    public int AuditedTransactions { get; set; }
    /// <summary>
    /// Gets or sets the collection of identified compliance gaps in the audit trail.
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the percentage of transactions that meet audit trail compliance requirements.
    /// </summary>
    public string CompliancePercentage { get; set; } = string.Empty;
}

/// <summary>
/// Represents an internal control deficiency identified during SOX compliance assessment.
/// </summary>
public class InternalControlDeficiencyDto
{
    /// <summary>
    /// Gets or sets the name of the internal control that has a deficiency.
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the type of deficiency identified (e.g., Design, Operating Effectiveness).
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the internal control deficiency.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the deficiency (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the root cause analysis of the internal control deficiency.
    /// </summary>
    public string RootCause { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the impact assessment of the deficiency on financial reporting.
    /// </summary>
    public string ImpactAssessment { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the internal control deficiency was identified.
    /// </summary>
    public DateTime IdentifiedDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who identified the deficiency.
    /// </summary>
    public string IdentifiedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents a formal deficiency report for SOX compliance issues.
/// </summary>
public class DeficiencyReportDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the deficiency report.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the control associated with the deficiency.
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the type of deficiency being reported (e.g., Design, Operating Effectiveness).
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed description of the deficiency being reported.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the reported deficiency (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the current status of the deficiency report (e.g., Open, In Progress, Resolved).
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the deficiency was formally reported.
    /// </summary>
    public DateTime ReportedDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who reported the deficiency.
    /// </summary>
    public string ReportedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the deficiency remediation was completed.
    /// </summary>
    public DateTime? RemediationDate { get; set; }
    /// <summary>
    /// Gets or sets the plan for remediating the reported deficiency.
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
}

/// <summary>
/// Represents compliance testing results for SOX internal controls.
/// </summary>
public class ComplianceTestingDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the compliance testing record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the control being tested for compliance.
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the procedure used to test the control's effectiveness.
    /// </summary>
    public string TestProcedure { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the compliance testing was performed.
    /// </summary>
    public DateTime TestDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who performed the compliance testing.
    /// </summary>
    public string TestPerformedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed results of the compliance testing.
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the compliance test passed successfully.
    /// </summary>
    public bool TestPassed { get; set; }
    /// <summary>
    /// Gets or sets the collection of exceptions identified during compliance testing.
    /// </summary>
    public IEnumerable<string> Exceptions { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the recommended actions to address any testing failures or exceptions.
    /// </summary>
    public string RecommendedActions { get; set; } = string.Empty;
}

/// <summary>
/// Represents a management assertion regarding internal control effectiveness for SOX compliance.
/// </summary>
public class ManagementAssertionDto
{
    /// <summary>
    /// Gets or sets the control objective that management is asserting about.
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the formal assertion statement made by management.
    /// </summary>
    public string AssertionStatement { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets management's response regarding the control's effectiveness.
    /// </summary>
    public string ManagementResponse { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the management assertion was made.
    /// </summary>
    public DateTime AssertionDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the management representative who made the assertion.
    /// </summary>
    public string AssertedBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of supporting evidence for the management assertion.
    /// </summary>
    public IEnumerable<string> SupportingEvidence { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets a value indicating whether management asserts the control is effective.
    /// </summary>
    public bool IsEffective { get; set; }
}

/// <summary>
/// Represents a request for remediation of SOX compliance deficiencies.
/// </summary>
public class RemediationRequestDto
{
    /// <summary>
    /// Gets or sets the identifier of the deficiency requiring remediation.
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed plan for remediating the compliance deficiency.
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the target date for completing the remediation.
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the party responsible for executing the remediation.
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of resources required to complete the remediation.
    /// </summary>
    public IEnumerable<string> RequiredResources { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the assessment of business impact during the remediation process.
    /// </summary>
    public string BusinessImpact { get; set; } = string.Empty;
}

/// <summary>
/// Represents a formal remediation plan for addressing SOX compliance deficiencies.
/// </summary>
public class RemediationPlanDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the remediation plan.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the identifier of the deficiency being addressed by this plan.
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed remediation plan for addressing the compliance deficiency.
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the target date for completing the remediation plan.
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the party responsible for executing the remediation plan.
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the current status of the remediation plan (e.g., Planned, In Progress, Completed).
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the remediation plan was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Gets or sets the date when the remediation plan was completed.
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    /// <summary>
    /// Gets or sets the notes documenting the completion of the remediation plan.
    /// </summary>
    public string CompletionNotes { get; set; } = string.Empty;
}
