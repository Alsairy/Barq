namespace BARQ.Core.Models.DTOs;

/// <summary>
/// PHI access log data transfer object for HIPAA compliance tracking
/// </summary>
public class PhiAccessLogDto
{
    /// <summary>
    /// Unique identifier of the user accessing PHI
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Unique identifier of the patient whose PHI is being accessed
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// Type of access performed (read, write, update, delete)
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// Specific resource or data element that was accessed
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// Business purpose or reason for accessing the PHI
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the PHI access occurred
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// IP address from which the access was made
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// User agent string of the client used for access
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}

/// <summary>
/// PHI access audit data transfer object for comprehensive HIPAA compliance auditing
/// </summary>
public class PhiAccessAuditDto
{
    /// <summary>
    /// Unique identifier for the audit record (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Unique identifier of the user accessing PHI
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Unique identifier of the patient whose PHI is being accessed
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// Type of access performed (read, write, update, delete)
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// Specific resource or data element that was accessed
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// Business purpose or reason for accessing the PHI
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the PHI access occurred
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// IP address from which the access was made
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// User agent string of the client used for access
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
    /// <summary>
    /// Authorization level of the user at the time of access
    /// </summary>
    public string AuthorizationLevel { get; set; } = string.Empty;
}

/// <summary>
/// Business associate request data transfer object for HIPAA compliance tracking
/// </summary>
public class BusinessAssociateRequestDto
{
    /// <summary>
    /// Name of the organization requesting business associate status
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// Primary contact person for the business associate request
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;
    /// <summary>
    /// Contact email address for the business associate request
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
    /// <summary>
    /// Description of services to be provided by the business associate
    /// </summary>
    public string ServicesProvided { get; set; } = string.Empty;
    /// <summary>
    /// Types of PHI that the business associate will access
    /// </summary>
    public IEnumerable<string> PhiTypesAccessed { get; set; } = new List<string>();
    /// <summary>
    /// Date when the business associate request was submitted
    /// </summary>
    public DateTime RequestDate { get; set; }
}

/// <summary>
/// Business associate agreement data transfer object for HIPAA compliance management
/// </summary>
public class BusinessAssociateAgreementDto
{
    /// <summary>
    /// Unique identifier for the business associate agreement
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the organization covered by the agreement
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// Unique agreement number for tracking purposes
    /// </summary>
    public string AgreementNumber { get; set; } = string.Empty;
    /// <summary>
    /// Date when the agreement becomes effective
    /// </summary>
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// Date when the agreement expires
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Current status of the agreement (active, expired, terminated)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// List of permitted uses of PHI under this agreement
    /// </summary>
    public IEnumerable<string> PermittedUses { get; set; } = new List<string>();
    /// <summary>
    /// Required safeguards that must be implemented by the business associate
    /// </summary>
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
