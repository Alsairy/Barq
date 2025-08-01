namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class FinancialControlsAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime FromDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ToDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsEffective { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> TestedControls { get; set; } = new List<string>();
    
    /// <summary>
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
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    
    /// <summary>
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
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string SystemAffected { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ApprovalDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ImplementationDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string BusinessJustification { get; set; } = string.Empty;
    
    /// <summary>
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
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime AuditDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string AuditorName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> UsersReviewed { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> AccessViolations { get; set; } = new List<string>();
    
    /// <summary>
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
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool HasDocumentation { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsCurrentVersion { get; set; }
    
    /// <summary>
    /// </summary>
    public bool HasApprovalSignatures { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastReviewDate { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> MissingDocuments { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string ComplianceStatus { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class InternalControlsAssessmentDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string ControlArea { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ControlDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ControlOwner { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ControlFrequency { get; set; } = string.Empty;
    
    /// <summary>
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
/// </summary>
public class AuditTrailComplianceDto
{
    /// <summary>
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime FromDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ToDate { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsCompliant { get; set; }
    
    /// <summary>
    /// </summary>
    public int TotalTransactions { get; set; }
    
    /// <summary>
    /// </summary>
    public int AuditedTransactions { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string CompliancePercentage { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class InternalControlDeficiencyDto
{
    /// <summary>
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RootCause { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ImpactAssessment { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime IdentifiedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string IdentifiedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class DeficiencyReportDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string DeficiencyType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime ReportedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string ReportedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime? RemediationDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ComplianceTestingDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string ControlName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string TestProcedure { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime TestDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string TestPerformedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool TestPassed { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> Exceptions { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string RecommendedActions { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ManagementAssertionDto
{
    /// <summary>
    /// </summary>
    public string ControlObjective { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string AssertionStatement { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ManagementResponse { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime AssertionDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string AssertedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> SupportingEvidence { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public bool IsEffective { get; set; }
}

/// <summary>
/// </summary>
public class RemediationRequestDto
{
    /// <summary>
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> RequiredResources { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public string BusinessImpact { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class RemediationPlanDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string DeficiencyId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RemediationPlan { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string ResponsibleParty { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string CompletionNotes { get; set; } = string.Empty;
}
