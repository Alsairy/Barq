namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class PhiAccessLogDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who accessed the PHI.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the patient whose PHI was accessed, if applicable.
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// Gets or sets the type of access performed (e.g., read, write, delete).
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the specific resource or data element that was accessed.
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the business purpose or justification for accessing the PHI.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp when the PHI access occurred.
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the PHI access was performed.
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the user agent string of the client used to access the PHI.
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class PhiAccessAuditDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this audit record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the user who accessed the PHI.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the patient whose PHI was accessed, if applicable.
    /// </summary>
    public Guid? PatientId { get; set; }
    /// <summary>
    /// Gets or sets the type of access performed (e.g., read, write, delete).
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the specific resource or data element that was accessed.
    /// </summary>
    public string ResourceAccessed { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the business purpose or justification for accessing the PHI.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp when the PHI access occurred.
    /// </summary>
    public DateTime AccessTime { get; set; }
    /// <summary>
    /// Gets or sets the IP address from which the PHI access was performed.
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the user agent string of the client used to access the PHI.
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the authorization level or permissions granted for this access.
    /// </summary>
    public string AuthorizationLevel { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class BusinessAssociateRequestDto
{
    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the primary contact person for the business associate request.
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address of the primary contact person.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a description of the services that will be provided by the business associate.
    /// </summary>
    public string ServicesProvided { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of PHI types that the business associate will need to access.
    /// </summary>
    public IEnumerable<string> PhiTypesAccessed { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date when the business associate request was submitted.
    /// </summary>
    public DateTime RequestDate { get; set; }
}

/// <summary>
/// </summary>
public class BusinessAssociateAgreementDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the unique agreement number for the Business Associate Agreement.
    /// </summary>
    public string AgreementNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the effective date when the agreement becomes active.
    /// </summary>
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// Gets or sets the expiration date when the agreement expires.
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Gets or sets the current status of the record.
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
    /// Gets or sets the type of security incident that occurred.
    /// </summary>
    public string IncidentType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the item or assessment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who discovered the breach.
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
    /// Gets or sets the current status of the record.
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
    /// Gets or sets the unique identifier for this record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the system being assessed for risk.
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the item or assessment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the assessed risk level (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public IEnumerable<string> IdentifiedThreats { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> Vulnerabilities { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of security safeguards implemented for the system.
    /// </summary>
    public IEnumerable<string> Safeguards { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date when the assessment was conducted.
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who conducted the assessment.
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
    /// Gets or sets the type of data breach that occurred.
    /// </summary>
    public string BreachType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the item or assessment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who discovered the breach.
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the estimated number of individuals affected by the breach.
    /// </summary>
    public int EstimatedAffectedIndividuals { get; set; }
    /// <summary>
    /// Gets or sets the collection of PHI types involved in the breach.
    /// </summary>
    public IEnumerable<string> PhiTypesInvolved { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the assessed risk level (e.g., Low, Medium, High, Critical).
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
    /// Gets or sets the unique identifier for this record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the title of the HIPAA training program.
    /// </summary>
    public string TrainingTitle { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the training was completed.
    /// </summary>
    public DateTime CompletionDate { get; set; }
    /// <summary>
    /// Gets or sets the certification number for the training completion.
    /// </summary>
    public string CertificationNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the expiration date when the agreement expires.
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
    /// Gets or sets the type of contingency plan (e.g., Data Backup, Disaster Recovery, Emergency Response).
    /// </summary>
    public string PlanType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name of the contingency plan.
    /// </summary>
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the item or assessment.
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
