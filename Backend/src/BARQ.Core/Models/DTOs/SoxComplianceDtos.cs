namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class FinancialControlsAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Start date of the audit period
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date of the audit period
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool IsEffective { get; set; }
    /// <summary>
    /// Collection of financial controls that were tested during the audit
    /// </summary>
    public IEnumerable<string> TestedControls { get; set; } = new List<string>();
    /// <summary>
    /// Collection of identified control deficiencies or weaknesses
    /// </summary>
    public IEnumerable<string> Deficiencies { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public string AuditorName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime AuditDate { get; set; }
}

/// <summary>
/// </summary>
public class ChangeControlLogDto
{
    /// <summary>
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
    /// <summary>
    /// System or application affected by the change
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the person who approved the change
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    /// <summary>
    /// Date when the change was implemented in the target system
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    /// <summary>
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    /// <summary>
    /// Risk assessment documenting potential impacts of the change
    /// </summary>
    public string RiskAssessment { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ChangeControlAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Type of change that was audited
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the person who originally requested the change
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }
    /// <summary>
    /// Date when the change was approved
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    /// <summary>
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    /// <summary>
    /// Risk assessment documentation for the change
    /// </summary>
    public string RiskAssessment { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool ComplianceStatus { get; set; }
}

/// <summary>
/// </summary>
public class AccessControlsAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the system being audited for access controls
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime AuditDate { get; set; }
    /// <summary>
    /// </summary>
    public string AuditorName { get; set; } = string.Empty;
    /// <summary>
    /// Collection of users whose access was reviewed during the audit
    /// </summary>
    public IEnumerable<string> UsersReviewed { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> AccessViolations { get; set; } = new List<string>();
    /// <summary>
    /// Collection of recommended actions to address access control issues
    /// </summary>
    public IEnumerable<string> RecommendedActions { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public bool OverallCompliance { get; set; }
}

/// <summary>
/// </summary>
public class DocumentationComplianceDto
{
    /// <summary>
    /// Name of the business process being assessed for documentation compliance
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the process has proper documentation in place
    /// </summary>
    public bool HasDocumentation { get; set; }
    /// <summary>
    /// Indicates whether the documentation is the current version
    /// </summary>
    public bool IsCurrentVersion { get; set; }
    /// <summary>
    /// Indicates whether the documentation has required approval signatures
    /// </summary>
    public bool HasApprovalSignatures { get; set; }
    /// <summary>
    /// Date when the documentation was last reviewed
    /// </summary>
    public DateTime LastReviewDate { get; set; }
    /// <summary>
    /// Collection of documents that are missing for compliance
    /// </summary>
    public IEnumerable<string> MissingDocuments { get; set; } = new List<string>();
    /// <summary>
    /// Overall compliance status of the documentation
    /// </summary>
    public string ComplianceStatus { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for internal controls assessment and effectiveness evaluation
/// </summary>
public class InternalControlsAssessmentDto
{
    /// <summary>
    /// Unique identifier for the internal controls assessment
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Area of control being assessed
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    /// <summary>
    /// Objective or goal of the control being assessed
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the control being assessed (important-comment)
    /// </summary>
    public string ControlDescription { get; set; } = string.Empty;
    /// <summary>
    /// Person or department responsible for the control
    /// </summary>
    public string ControlOwner { get; set; } = string.Empty;
    /// <summary>
    /// Frequency at which the control is executed
    /// </summary>
    public string ControlFrequency { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the control is operating effectively
    /// </summary>
    public bool IsEffective { get; set; }
    /// <summary>
    /// Procedure used to test the control effectiveness
    /// </summary>
    public string TestingProcedure { get; set; } = string.Empty;
    /// <summary>
    /// Date when the control was last tested
    /// </summary>
    public DateTime LastTested { get; set; }
    /// <summary>
    /// Results of the control testing
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for audit trail compliance assessment and transaction monitoring
/// </summary>
public class AuditTrailComplianceDto
{
    /// <summary>
    /// Name of the system being assessed for audit trail compliance (important-comment)
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Start date of the audit trail assessment period
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date of the audit trail assessment period
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Indicates whether the audit trail meets compliance requirements
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Total number of transactions in the assessment period
    /// </summary>
    public int TotalTransactions { get; set; }
    /// <summary>
    /// Number of transactions that were audited
    /// </summary>
    public int AuditedTransactions { get; set; }
    /// <summary>
    /// Collection of identified compliance gaps in the audit trail
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    /// <summary>
    /// Percentage of transactions that meet compliance requirements
    /// </summary>
    public string CompliancePercentage { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for internal control deficiencies and weaknesses identification
/// </summary>
public class InternalControlDeficiencyDto
{
    /// <summary>
    /// Name of the control that has the deficiency
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Type or category of the control deficiency
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the control deficiency (important-comment)
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the control deficiency
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Root cause analysis of the control deficiency
    /// </summary>
    public string RootCause { get; set; } = string.Empty;
    /// <summary>
    /// Assessment of the potential impact of the deficiency
    /// </summary>
    public string ImpactAssessment { get; set; } = string.Empty;
    /// <summary>
    /// Date when the deficiency was identified
    /// </summary>
    public DateTime IdentifiedDate { get; set; }
    /// <summary>
    /// Person or team who identified the deficiency
    /// </summary>
    public string IdentifiedBy { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for deficiency reporting and tracking
/// </summary>
public class DeficiencyReportDto
{
    /// <summary>
    /// Unique identifier for the deficiency report
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the control with the reported deficiency
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Type or category of the reported deficiency
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the reported deficiency
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the reported deficiency
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Current status of the deficiency report
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Date when the deficiency was reported
    /// </summary>
    public DateTime ReportedDate { get; set; }
    /// <summary>
    /// Person or team who reported the deficiency
    /// </summary>
    public string ReportedBy { get; set; } = string.Empty;
    /// <summary>
    /// Date when the deficiency was remediated
    /// </summary>
    public DateTime? RemediationDate { get; set; }
    /// <summary>
    /// Plan for remediating the reported deficiency
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for compliance testing procedures and results
/// </summary>
public class ComplianceTestingDto
{
    /// <summary>
    /// Unique identifier for the compliance test
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the control being tested
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    /// <summary>
    /// Procedure used to test the control
    /// </summary>
    public string TestProcedure { get; set; } = string.Empty;
    /// <summary>
    /// Date when the compliance test was performed
    /// </summary>
    public DateTime TestDate { get; set; }
    /// <summary>
    /// Person or team who performed the test
    /// </summary>
    public string TestPerformedBy { get; set; } = string.Empty;
    /// <summary>
    /// Results of the compliance test
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether the compliance test passed
    /// </summary>
    public bool TestPassed { get; set; }
    /// <summary>
    /// Collection of exceptions found during testing
    /// </summary>
    public IEnumerable<string> Exceptions { get; set; } = new List<string>();
    /// <summary>
    /// Recommended actions based on test results
    /// </summary>
    public string RecommendedActions { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for management assertions on internal control effectiveness
/// </summary>
public class ManagementAssertionDto
{
    /// <summary>
    /// Objective of the control being asserted
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    /// <summary>
    /// Management's assertion statement about control effectiveness
    /// </summary>
    public string AssertionStatement { get; set; } = string.Empty;
    /// <summary>
    /// Management's response regarding the control
    /// </summary>
    public string ManagementResponse { get; set; } = string.Empty;
    /// <summary>
    /// Date when the assertion was made
    /// </summary>
    public DateTime AssertionDate { get; set; }
    /// <summary>
    /// Person who made the management assertion
    /// </summary>
    public string AssertedBy { get; set; } = string.Empty;
    /// <summary>
    /// Collection of evidence supporting the assertion
    /// </summary>
    public IEnumerable<string> SupportingEvidence { get; set; } = new List<string>();
    /// <summary>
    /// Indicates whether management asserts the control is effective
    /// </summary>
    public bool IsEffective { get; set; }
}

/// <summary>
/// Data transfer object for remediation requests and planning
/// </summary>
public class RemediationRequestDto
{
    /// <summary>
    /// Identifier of the deficiency requiring remediation
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    /// <summary>
    /// Plan for remediating the identified deficiency
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    /// <summary>
    /// Target date for completing the remediation
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    /// <summary>
    /// Person or team responsible for executing the remediation
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    /// <summary>
    /// Collection of resources required for remediation
    /// </summary>
    public IEnumerable<string> RequiredResources { get; set; } = new List<string>();
    /// <summary>
    /// Assessment of business impact during remediation
    /// </summary>
    public string BusinessImpact { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for remediation plan tracking and execution
/// </summary>
public class RemediationPlanDto
{
    /// <summary>
    /// Unique identifier for the remediation plan
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Identifier of the deficiency being remediated
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    /// <summary>
    /// Detailed plan for remediating the deficiency
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    /// <summary>
    /// Target date for completing the remediation plan
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    /// <summary>
    /// Person or team responsible for the remediation plan
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    /// <summary>
    /// Current status of the remediation plan
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Date when the remediation plan was created
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Date when the remediation plan was completed
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    /// <summary>
    /// Notes documenting the completion of the remediation plan
    /// </summary>
    public string CompletionNotes { get; set; } = string.Empty;
}
