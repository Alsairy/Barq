namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class PhiAccessLogDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class PhiAccessAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string AuthorizationLevel { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class BusinessAssociateRequestDto
{
    /// <summary>
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ServicesProvided { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> PhiTypesAccessed { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }
}

/// <summary>
/// </summary>
public class BusinessAssociateAgreementDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string AgreementNumber { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> PermittedUses { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> RequiredSafeguards { get; set; } = new List<string>();
}

/// <summary>
/// </summary>
public class EncryptionComplianceDto
{
    /// <summary>
    /// </summary>
    public string DataLocation { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string EncryptionMethod { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// </summary>
    public string ComplianceLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public DateTime ValidatedAt { get; set; }
}

/// <summary>
/// </summary>
public class SecurityIncidentDto
{
    /// <summary>
    /// </summary>
    public string IncidentType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> AffectedSystems { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public bool PhiInvolved { get; set; }
    /// <summary>
    /// </summary>
    public string ImmediateActions { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class SecurityIncidentResponseDto
{
    /// <summary>
    /// </summary>
    public Guid IncidentId { get; set; }
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ResponsePlan { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> ActionsRequired { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public DateTime ResponseInitiated { get; set; }
    /// <summary>
    /// </summary>
    public string ResponseTeam { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class RiskAssessmentDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> IdentifiedThreats { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> Vulnerabilities { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> Safeguards { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class AuditLogComplianceDto
{
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
    public int TotalLogEntries { get; set; }
    /// <summary>
    /// </summary>
    public int ComplianceViolations { get; set; }
    /// <summary>
    /// </summary>
    public IEnumerable<string> ViolationTypes { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public string ComplianceScore { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class HipaaBreachReportDto
{
    /// <summary>
    /// </summary>
    public string BreachType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public int EstimatedAffectedIndividuals { get; set; }
    /// <summary>
    /// </summary>
    public IEnumerable<string> PhiTypesInvolved { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool RequiresHhsNotification { get; set; }
    /// <summary>
    /// </summary>
    public bool RequiresMediaNotification { get; set; }
}

/// <summary>
/// </summary>
public class WorkforceTrainingDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string TrainingTitle { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime CompletionDate { get; set; }
    /// <summary>
    /// </summary>
    public string CertificationNumber { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// </summary>
    public string TrainingProvider { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ContingencyPlanDto
{
    /// <summary>
    /// </summary>
    public string PlanType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> Procedures { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> ResponsiblePersons { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
    /// <summary>
    /// </summary>
    public DateTime LastTested { get; set; }
    /// <summary>
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
}
