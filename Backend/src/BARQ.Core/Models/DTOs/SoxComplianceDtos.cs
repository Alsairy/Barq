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
    public bool HasDocumentation { get; set; }
    public bool IsCurrentVersion { get; set; }
    public bool HasApprovalSignatures { get; set; }
    public DateTime LastReviewDate { get; set; }
    public IEnumerable<string> MissingDocuments { get; set; } = new List<string>();
    public string ComplianceStatus { get; set; } = string.Empty;
}

public class InternalControlsAssessmentDto
{
    public Guid Id { get; set; }
    public string ControlArea { get; set; } = string.Empty;
    public string ControlObjective { get; set; } = string.Empty;
    public string ControlDescription { get; set; } = string.Empty;
    public string ControlOwner { get; set; } = string.Empty;
    public string ControlFrequency { get; set; } = string.Empty;
    public bool IsEffective { get; set; }
    public string TestingProcedure { get; set; } = string.Empty;
    public DateTime LastTested { get; set; }
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
