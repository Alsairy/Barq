namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class DataSubjectRequestDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// </summary>
    public string RequestType { get; set; } = string.Empty; // Access, Rectification, Erasure, Portability, Restriction

    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string IdentityVerification { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class DataSubjectRightsResponseDto
{
    /// <summary>
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ResponseData { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public DateTime ProcessedAt { get; set; }

    /// <summary>
    /// </summary>
    public string ProcessedBy { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ConsentUpdateRequestDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool ConsentGiven { get; set; }
    /// <summary>
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime ConsentDate { get; set; }
    /// <summary>
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ConsentManagementResponseDto
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// </summary>
    public string ConsentId { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ConsentStatusDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public bool ConsentGiven { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ConsentDate { get; set; }
    /// <summary>
    /// </summary>
    public DateTime? WithdrawnDate { get; set; }
    /// <summary>
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class DataPortabilityResponseDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string Format { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string DataPackage { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime ExportedAt { get; set; }
    /// <summary>
    /// </summary>
    public string ExportedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public long DataSize { get; set; }
}

/// <summary>
/// </summary>
public class DataErasureResponseDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// </summary>
    public IEnumerable<string> ErasedDataTypes { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public IEnumerable<string> RetainedDataTypes { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public string RetentionReason { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime ErasedAt { get; set; }
    /// <summary>
    /// </summary>
    public string ErasedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class DataProcessingAuditDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string ProcessingActivity { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime ProcessedAt { get; set; }
    /// <summary>
    /// </summary>
    public string ProcessedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string DataTypes { get; set; } = string.Empty;
}

/// <summary>
/// Privacy Impact Assessment data transfer object for GDPR compliance
/// </summary>
public class PrivacyImpactAssessmentDto
{
    /// <summary>
    /// Unique identifier for the privacy impact assessment
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of the process being assessed
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the privacy impact assessment
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Risk level determined by the assessment
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// List of identified privacy risks
    /// </summary>
    public IEnumerable<string> IdentifiedRisks { get; set; } = new List<string>();
    
    /// <summary>
    /// List of mitigation measures for identified risks
    /// </summary>
    public IEnumerable<string> Mitigations { get; set; } = new List<string>();
    
    /// <summary>
    /// Date when the assessment was conducted
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    
    /// <summary>
    /// Person who conducted the assessment
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// Lawful basis validation data transfer object for GDPR compliance
/// </summary>
public class LawfulBasisValidationDto
{
    /// <summary>
    /// The data processing activity being validated
    /// </summary>
    public string ProcessingActivity { get; set; } = string.Empty;
    
    /// <summary>
    /// The lawful basis claimed for the processing activity
    /// </summary>
    public string LawfulBasis { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the lawful basis is valid for the processing activity
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Reason for the validation result
    /// </summary>
    public string ValidationReason { get; set; } = string.Empty;
    
    /// <summary>
    /// Date and time when the validation was performed
    /// </summary>
    public DateTime ValidatedAt { get; set; }
}

/// <summary>
/// Data retention policy data transfer object for GDPR compliance
/// </summary>
public class DataRetentionPolicyDto
{
    /// <summary>
    /// Type of data covered by this retention policy
    /// </summary>
    public string DataType { get; set; } = string.Empty;
    /// <summary>
    /// Period for which the data should be retained
    /// </summary>
    public TimeSpan RetentionPeriod { get; set; }
    /// <summary>
    /// Reason for retaining the data for this period
    /// </summary>
    public string RetentionReason { get; set; } = string.Empty;
    /// <summary>
    /// Method used to dispose of data after retention period
    /// </summary>
    public string DisposalMethod { get; set; } = string.Empty;
    /// <summary>
    /// Date when the policy was created
    /// </summary>
    public DateTime PolicyCreated { get; set; }
    /// <summary>
    /// Date when the policy was last updated
    /// </summary>
    public DateTime PolicyUpdated { get; set; }
}

/// <summary>
/// Breach notification data transfer object for GDPR compliance
/// </summary>
public class BreachNotificationDto
{
    /// <summary>
    /// Unique identifier for the data breach
    /// </summary>
    public Guid BreachId { get; set; }
    /// <summary>
    /// Type of notification being sent
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    /// <summary>
    /// Recipient of the breach notification
    /// </summary>
    public string Recipient { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the notification was sent
    /// </summary>
    public DateTime NotifiedAt { get; set; }
    /// <summary>
    /// Method used to send the notification
    /// </summary>
    public string NotificationMethod { get; set; } = string.Empty;
    /// <summary>
    /// Current status of the notification
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Data breach report data transfer object for GDPR compliance
/// </summary>
public class DataBreachReportDto
{
    /// <summary>
    /// Type of data breach that occurred
    /// </summary>
    public string BreachType { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the data breach
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the breach was discovered
    /// </summary>
    public DateTime DiscoveredAt { get; set; }
    /// <summary>
    /// Person who discovered the breach
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;
    /// <summary>
    /// Types of data affected by the breach
    /// </summary>
    public IEnumerable<string> AffectedDataTypes { get; set; } = new List<string>();
    /// <summary>
    /// Estimated number of users affected by the breach
    /// </summary>
    public int EstimatedAffectedUsers { get; set; }
    /// <summary>
    /// Risk level assessment of the breach
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// Measures taken to contain the breach
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
}

/// <summary>
/// Consent record data transfer object for GDPR compliance
/// </summary>
public class ConsentRecordDto
{
    /// <summary>
    /// Unique identifier for the consent record
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;
    /// <summary>
    /// Whether consent was given or withdrawn
    /// </summary>
    public bool ConsentGiven { get; set; }
    /// <summary>
    /// Date when consent was given
    /// </summary>
    public DateTime ConsentDate { get; set; }
    /// <summary>
    /// Date when consent was withdrawn, if applicable
    /// </summary>
    public DateTime? WithdrawnDate { get; set; }
    /// <summary>
    /// Purpose for which consent was given
    /// </summary>
    public string Purpose { get; set; } = string.Empty;
    /// <summary>
    /// Legal basis for processing the data
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
    /// <summary>
    /// Method used to obtain consent
    /// </summary>
    public string ConsentMethod { get; set; } = string.Empty;
}

/// <summary>
/// Key escrow data transfer object for GDPR compliance
/// </summary>
public class KeyEscrowDto
{
    /// <summary>
    /// Unique identifier for the key escrow
    /// </summary>
    public string EscrowId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    /// <summary>
    /// Reason for key escrow
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the escrow was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date and time when the escrow expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    /// <summary>
    /// Person who created the escrow
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
    /// <summary>
    /// Current status of the escrow
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Authorization required to access the escrowed key
    /// </summary>
    public string AuthorizationRequired { get; set; } = string.Empty;
}

/// <summary>
/// Key usage audit data transfer object for GDPR compliance
/// </summary>
public class KeyUsageAuditDto
{
    /// <summary>
    /// Unique identifier for the audit record
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Identifier of the key that was used
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    /// <summary>
    /// Operation performed with the key
    /// </summary>
    public string Operation { get; set; } = string.Empty;
    /// <summary>
    /// Type of entity the key was used on
    /// </summary>
    public string EntityType { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the entity the key was used on
    /// </summary>
    public Guid EntityId { get; set; }
    /// <summary>
    /// Date and time when the key was used
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Whether the key operation was successful
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// IP address from which the key operation was performed
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
}
