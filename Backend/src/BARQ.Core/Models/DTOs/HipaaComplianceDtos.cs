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

/// <summary>
/// Encryption compliance data transfer object for HIPAA security compliance tracking
/// </summary>
public class EncryptionComplianceDto
{
    /// <summary>
    /// Location where the data is stored (database, file system, cloud storage)
    /// </summary>
    public string DataLocation { get; set; } = string.Empty;
    /// <summary>
    /// Encryption method used to protect the data (AES-256, RSA, etc.)
    /// </summary>
    public string EncryptionMethod { get; set; } = string.Empty;
    /// <summary>
    /// Whether the encryption meets HIPAA compliance requirements
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Level of compliance achieved (full, partial, non-compliant)
    /// </summary>
    public string ComplianceLevel { get; set; } = string.Empty;
    /// <summary>
    /// List of identified gaps in encryption compliance
    /// </summary>
    public IEnumerable<string> ComplianceGaps { get; set; } = new List<string>();
    /// <summary>
    /// Date and time when the encryption compliance was last validated
    /// </summary>
    public DateTime ValidatedAt { get; set; }
}

/// <summary>
/// Security incident data transfer object for HIPAA security incident tracking
/// </summary>
public class SecurityIncidentDto
{
    /// <summary>
    /// Type of security incident (data breach, unauthorized access, malware, etc.) (important-comment)
    /// </summary>
    public string IncidentType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the security incident
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the incident was discovered
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Person or system that discovered the incident
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Severity level of the incident (low, medium, high, critical)
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// List of systems affected by the security incident
    /// </summary>
    public IEnumerable<string> AffectedSystems { get; set; } = new List<string>();
    /// <summary>
    /// Whether protected health information (PHI) was involved in the incident
    /// </summary>
    public bool PhiInvolved { get; set; }
    /// <summary>
    /// Immediate actions taken to contain or mitigate the incident
    /// </summary>
    public string ImmediateActions { get; set; } = string.Empty;
}

/// <summary>
/// Security incident response data transfer object for HIPAA incident response tracking
/// </summary>
public class SecurityIncidentResponseDto
{
    /// <summary>
    /// Unique identifier of the security incident being responded to
    /// </summary>
    public Guid IncidentId { get; set; }
    /// <summary>
    /// Current status of the incident response (initiated, in-progress, completed)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Detailed response plan for addressing the security incident
    /// </summary>
    public string ResponsePlan { get; set; } = string.Empty;
    /// <summary>
    /// List of actions required to complete the incident response
    /// </summary>
    public IEnumerable<string> ActionsRequired { get; set; } = new List<string>();
    /// <summary>
    /// Date and time when the incident response was initiated
    /// </summary>
    public DateTime ResponseInitiated { get; set; }
    /// <summary>
    /// Team or individuals responsible for the incident response
    /// </summary>
    public string ResponseTeam { get; set; } = string.Empty;
}

/// <summary>
/// Risk assessment data transfer object for HIPAA security risk evaluation
/// </summary>
public class RiskAssessmentDto
{
    /// <summary>
    /// Unique identifier for the risk assessment
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the system being assessed for security risks
    /// </summary>
    public string SystemName { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the risk assessment scope and methodology
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Overall risk level determined by the assessment (low, medium, high, critical)
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// List of security threats identified during the assessment
    /// </summary>
    public IEnumerable<string> IdentifiedThreats { get; set; } = new List<string>();
    /// <summary>
    /// List of vulnerabilities discovered in the assessed system
    /// </summary>
    public IEnumerable<string> Vulnerabilities { get; set; } = new List<string>();
    /// <summary>
    /// List of safeguards recommended or implemented to mitigate risks
    /// </summary>
    public IEnumerable<string> Safeguards { get; set; } = new List<string>();
    /// <summary>
    /// Date when the risk assessment was conducted
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// Person or team who conducted the risk assessment
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// Audit log compliance data transfer object for HIPAA audit trail compliance tracking
/// </summary>
public class AuditLogComplianceDto
{
    /// <summary>
    /// Start date of the audit log compliance assessment period
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date of the audit log compliance assessment period
    /// </summary>
    public DateTime ToDate { get; set; }
    /// <summary>
    /// Whether the audit logs meet HIPAA compliance requirements
    /// </summary>
    public bool IsCompliant { get; set; }
    /// <summary>
    /// Total number of log entries reviewed during the assessment
    /// </summary>
    public int TotalLogEntries { get; set; }
    /// <summary>
    /// Number of compliance violations found in the audit logs
    /// </summary>
    public int ComplianceViolations { get; set; }
    /// <summary>
    /// Types of compliance violations identified in the audit logs
    /// </summary>
    public IEnumerable<string> ViolationTypes { get; set; } = new List<string>();
    /// <summary>
    /// Overall compliance score for the audit log assessment
    /// </summary>
    public string ComplianceScore { get; set; } = string.Empty;
}

/// <summary>
/// HIPAA breach report data transfer object for breach notification and reporting
/// </summary>
public class HipaaBreachReportDto
{
    /// <summary>
    /// Type of HIPAA breach (unauthorized access, theft, loss, improper disposal)
    /// </summary>
    public string BreachType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the HIPAA breach incident
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the breach was discovered
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Person or system that discovered the breach
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Estimated number of individuals affected by the breach
    /// </summary>
    public int EstimatedAffectedIndividuals { get; set; }
    /// <summary>
    /// Types of protected health information involved in the breach
    /// </summary>
    public IEnumerable<string> PhiTypesInvolved { get; set; } = new List<string>();
    /// <summary>
    /// Risk level of the breach (low, medium, high, critical)
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// Measures taken to contain and mitigate the breach
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
    /// <summary>
    /// Whether the breach requires notification to the Department of Health and Human Services
    /// </summary>
    public bool RequiresHhsNotification { get; set; }
    /// <summary>
    /// Whether the breach requires media notification due to its scope
    /// </summary>
    public bool RequiresMediaNotification { get; set; }
}

/// <summary>
/// Workforce training data transfer object for HIPAA training compliance tracking
/// </summary>
public class WorkforceTrainingDto
{
    /// <summary>
    /// Unique identifier for the training record
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Unique identifier of the user who completed the training
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Type of HIPAA training completed (privacy, security, breach response)
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;
    /// <summary>
    /// Title of the specific training course or module
    /// </summary>
    public string TrainingTitle { get; set; } = string.Empty;
    /// <summary>
    /// Date when the training was completed
    /// </summary>
    public DateTime CompletionDate { get; set; }
    /// <summary>
    /// Certification number or identifier issued upon training completion
    /// </summary>
    public string CertificationNumber { get; set; } = string.Empty;
    /// <summary>
    /// Date when the training certification expires and renewal is required
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    /// <summary>
    /// Organization or entity that provided the training
    /// </summary>
    public string TrainingProvider { get; set; } = string.Empty;
}

/// <summary>
/// Contingency plan data transfer object for HIPAA emergency response and business continuity planning
/// </summary>
public class ContingencyPlanDto
{
    /// <summary>
    /// Type of contingency plan (disaster recovery, emergency response, business continuity)
    /// </summary>
    public string PlanType { get; set; } = string.Empty;
    /// <summary>
    /// Name of the contingency plan
    /// </summary>
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the contingency plan scope and objectives
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// List of procedures to be followed during contingency activation
    /// </summary>
    public IEnumerable<string> Procedures { get; set; } = new List<string>();
    /// <summary>
    /// List of persons responsible for executing the contingency plan
    /// </summary>
    public IEnumerable<string> ResponsiblePersons { get; set; } = new List<string>();
    /// <summary>
    /// Date when the contingency plan was last updated
    /// </summary>
    public DateTime LastUpdated { get; set; }
    /// <summary>
    /// Date when the contingency plan was last tested or exercised
    /// </summary>
    public DateTime LastTested { get; set; }
    /// <summary>
    /// Results and findings from the most recent contingency plan test
    /// </summary>
    public string TestResults { get; set; } = string.Empty;
}
