namespace BARQ.Core.Models.DTOs;

public class PhiAccessLogDto
{
    public Guid UserId { get; set; }
    public Guid? PatientId { get; set; }
    public string AccessType { get; set; } = string.Empty;
    public string ResourceAccessed { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime AccessTime { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

public class PhiAccessAuditDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? PatientId { get; set; }
    public string AccessType { get; set; } = string.Empty;
    public string ResourceAccessed { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime AccessTime { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string AuthorizationLevel { get; set; } = string.Empty;
}

public class BusinessAssociateRequestDto
{
    public string OrganizationName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ServicesProvided { get; set; } = string.Empty;
    public IEnumerable<string> PhiTypesAccessed { get; set; } = new List<string>();
    public DateTime RequestDate { get; set; }
}

public class BusinessAssociateAgreementDto
{
    public Guid Id { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string AgreementNumber { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public IEnumerable<string> PermittedUses { get; set; } = new List<string>();
    public IEnumerable<string> RequiredSafeguards { get; set; } = new List<string>();
}

public class EncryptionComplianceDto
{
    public string DataLocation { get; set; } = string.Empty;
    public string EncryptionMethod { get; set; } = string.Empty;
    public bool IsCompliant { get; set; }
    public string ComplianceLevel { get; set; } = string.Empty;
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    public DateTime ValidatedAt { get; set; }
}

public class SecurityIncidentDto
{
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DiscoveredAt { get; set; }
    public string DiscoveredBy { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public IEnumerable<string> AffectedSystems { get; set; } = new List<string>();
    public bool PhiInvolved { get; set; }
    public string ImmediateActions { get; set; } = string.Empty;
}

public class SecurityIncidentResponseDto
{
    public Guid IncidentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ResponsePlan { get; set; } = string.Empty;
    public IEnumerable<string> ActionsRequired { get; set; } = new List<string>();
    public DateTime ResponseInitiated { get; set; }
    public string ResponseTeam { get; set; } = string.Empty;
}

public class RiskAssessmentDto
{
    public Guid Id { get; set; }
    public string SystemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public IEnumerable<string> IdentifiedThreats { get; set; } = new List<string>();
    public IEnumerable<string> Vulnerabilities { get; set; } = new List<string>();
    public IEnumerable<string> Safeguards { get; set; } = new List<string>();
    public DateTime AssessmentDate { get; set; }
    public string AssessedBy { get; set; } = string.Empty;
}

public class AuditLogComplianceDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsCompliant { get; set; }
    public int TotalLogEntries { get; set; }
    public int ComplianceViolations { get; set; }
    public IEnumerable<string> ViolationTypes { get; set; } = new List<string>();
    public string ComplianceScore { get; set; } = string.Empty;
}

public class HipaaBreachReportDto
{
    public string BreachType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DiscoveredAt { get; set; }
    public string DiscoveredBy { get; set; } = string.Empty;
    public int EstimatedAffectedIndividuals { get; set; }
    public IEnumerable<string> PhiTypesInvolved { get; set; } = new List<string>();
    public string RiskLevel { get; set; } = string.Empty;
    public string ContainmentMeasures { get; set; } = string.Empty;
    public bool RequiresHhsNotification { get; set; }
    public bool RequiresMediaNotification { get; set; }
}

public class WorkforceTrainingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TrainingType { get; set; } = string.Empty;
    public string TrainingTitle { get; set; } = string.Empty;
    public DateTime CompletionDate { get; set; }
    public string CertificationNumber { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public string TrainingProvider { get; set; } = string.Empty;
}

public class ContingencyPlanDto
{
    public string PlanType { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<string> Procedures { get; set; } = new List<string>();
    public IEnumerable<string> ResponsiblePersons { get; set; } = new List<string>();
    public DateTime LastUpdated { get; set; }
    public DateTime LastTested { get; set; }
    public string TestResults { get; set; } = string.Empty;
}
