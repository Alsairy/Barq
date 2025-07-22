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
    /// Gets or sets the collection of permitted uses of PHI as defined in the Business Associate Agreement.
    /// </summary>
    public IEnumerable<string> PermittedUses { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of required safeguards that must be implemented to protect PHI.
    /// </summary>
    public IEnumerable<string> RequiredSafeguards { get; set; } = new List<string>();
}

/// <summary>
/// </summary>
public class EncryptionComplianceDto
{
    /// <summary>
    /// Gets or sets the location where the encrypted data is stored (e.g., Database, File System, Cloud Storage).
    /// </summary>
    public string DataLocation { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the encryption method used to protect the data (e.g., AES-256, RSA).
    /// </summary>
    public string EncryptionMethod { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the encryption implementation meets HIPAA compliance requirements.
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Gets or sets the level of compliance achieved (e.g., Basic, Standard, Advanced).
    /// </summary>
    public string ComplianceLevel { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of identified gaps or deficiencies in the encryption implementation.
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date and time when the encryption compliance was last validated.
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
    /// Gets or sets a detailed description of the security incident and its potential impact.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the security incident was discovered.
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who discovered the security incident.
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the severity level of the security incident (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of systems that were affected by the security incident.
    /// </summary>
    public IEnumerable<string> AffectedSystems { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets a value indicating whether Protected Health Information (PHI) was involved in the incident.
    /// </summary>
    public bool PhiInvolved { get; set; }
    /// <summary>
    /// Gets or sets the description of immediate actions taken to contain and mitigate the security incident.
    /// </summary>
    public string ImmediateActions { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class SecurityIncidentResponseDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the security incident being responded to.
    /// </summary>
    public Guid IncidentId { get; set; }
    /// <summary>
    /// Gets or sets the current status of the incident response (e.g., Initiated, In Progress, Resolved).
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the detailed response plan being executed to address the security incident.
    /// </summary>
    public string ResponsePlan { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of actions required to fully resolve the security incident.
    /// </summary>
    public IEnumerable<string> ActionsRequired { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date and time when the incident response was initiated.
    /// </summary>
    public DateTime ResponseInitiated { get; set; }
    /// <summary>
    /// Gets or sets the name of the team or individuals responsible for the incident response.
    /// </summary>
    public string ResponseTeam { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class RiskAssessmentDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this risk assessment record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the system being assessed for HIPAA security risks.
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the system and the scope of the risk assessment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the overall assessed risk level (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of security threats identified during the risk assessment.
    /// </summary>
    public IEnumerable<string> IdentifiedThreats { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of vulnerabilities discovered in the system during the assessment.
    /// </summary>
    public IEnumerable<string> Vulnerabilities { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of security safeguards implemented to protect PHI in the system.
    /// </summary>
    public IEnumerable<string> Safeguards { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date when the HIPAA risk assessment was conducted.
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who conducted the risk assessment.
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class AuditLogComplianceDto
{
    /// <summary>
    /// Gets or sets the start date of the audit log compliance assessment period.
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// Gets or sets the end date of the audit log compliance assessment period.
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the audit logs meet HIPAA compliance requirements.
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Gets or sets the total number of audit log entries reviewed during the assessment period.
    /// </summary>
    public int TotalLogEntries { get; set; }
    /// <summary>
    /// Gets or sets the number of compliance violations found in the audit logs.
    /// </summary>
    public int ComplianceViolations { get; set; }
    /// <summary>
    /// Gets or sets the collection of types of compliance violations identified in the audit logs.
    /// </summary>
    public IEnumerable<string> ViolationTypes { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the overall compliance score for the audit log assessment.
    /// </summary>
    public string ComplianceScore { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class HipaaBreachReportDto
{
    /// <summary>
    /// Gets or sets the type of HIPAA breach that occurred (e.g., Unauthorized Access, Data Theft, System Compromise).
    /// </summary>
    public string BreachType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the HIPAA breach incident and its circumstances.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date and time when the HIPAA breach was discovered.
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Gets or sets the name of the person who discovered the HIPAA breach.
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the estimated number of individuals whose PHI was affected by the breach.
    /// </summary>
    public int EstimatedAffectedIndividuals { get; set; }
    /// <summary>
    /// Gets or sets the collection of PHI types that were involved in the breach incident.
    /// </summary>
    public IEnumerable<string> PhiTypesInvolved { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the assessed risk level of the HIPAA breach (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the description of measures taken to contain and mitigate the breach.
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a value indicating whether the breach requires notification to the Department of Health and Human Services (HHS).
    /// </summary>
    public bool RequiresHhsNotification { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the breach requires public media notification due to its scope and impact.
    /// </summary>
    public bool RequiresMediaNotification { get; set; }
}

/// <summary>
/// </summary>
public class WorkforceTrainingDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this training record.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the user who completed the HIPAA training.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Gets or sets the type of HIPAA training completed (e.g., Privacy, Security, Breach Response).
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the title of the HIPAA training program.
    /// </summary>
    public string TrainingTitle { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the date when the HIPAA training was completed.
    /// </summary>
    public DateTime CompletionDate { get; set; }
    /// <summary>
    /// Gets or sets the certification number issued upon successful completion of the training.
    /// </summary>
    public string CertificationNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the expiration date when the training certification expires and renewal is required.
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Gets or sets the name of the organization or entity that provided the HIPAA training.
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
    /// Gets or sets the name of the HIPAA contingency plan.
    /// </summary>
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets a detailed description of the contingency plan and its scope.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the collection of procedures to be followed when executing the contingency plan.
    /// </summary>
    public IEnumerable<string> Procedures { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the collection of individuals responsible for implementing and maintaining the contingency plan.
    /// </summary>
    public IEnumerable<string> ResponsiblePersons { get; set; } = new List<string>();
    /// <summary>
    /// Gets or sets the date and time when the contingency plan was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the contingency plan was last tested for effectiveness.
    /// </summary>
    public DateTime LastTested { get; set; }
    /// <summary>
    /// Gets or sets the results and findings from the most recent contingency plan test.
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
}
