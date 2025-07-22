namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Represents a request made by a data subject under GDPR, including the type of request and related details.
/// </summary>
public class DataSubjectRequestDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user making the request.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the type of request (e.g., Access, Rectification, Erasure, Portability, Restriction).
    /// </summary>
    public string RequestType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a description of the request provided by the user.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets information used to verify the identity of the data subject.
    /// </summary>
    public string IdentityVerification { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the request was made.
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// Gets or sets the contact email address for the data subject.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;
}

/// <summary>
/// Represents a response to a data subject rights request, including status and processing details.
/// </summary>
public class DataSubjectRightsResponseDto
{
    /// <summary>
    /// Gets or sets the identifier of the original request.
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Gets or sets the status of the response (e.g., Pending, Completed, Denied).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any data returned as part of the response.
    /// </summary>
    public string ResponseData { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the request was processed.
    /// </summary>
    public DateTime ProcessedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier or name of the person or system that processed the request.
    /// </summary>
    public string ProcessedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any notes associated with processing the request.
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents a request to update a user's consent, including the type and purpose of consent.
/// </summary>
public class ConsentUpdateRequestDto
{
    /// <summary>
    /// Gets or sets the user identifier for whom consent is being updated.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the type of consent being updated.
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether consent has been granted.
    /// </summary>
    public bool ConsentGiven { get; set; }

    /// <summary>
    /// Gets or sets the purpose for which consent is being sought.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when consent was given.
    /// </summary>
    public DateTime ConsentDate { get; set; }

    /// <summary>
    /// Gets or sets the legal basis for processing the data.
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
}

/// <summary>
/// Represents the result of a consent management operation.
/// </summary>
public class ConsentManagementResponseDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message describing the outcome of the operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the consent was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the consent record.
    /// </summary>
    public string ConsentId { get; set; } = string.Empty;
}

/// <summary>
/// Represents the current status of a user's consent, including type, dates, and legal basis.
/// </summary>
public class ConsentStatusDto
{
    /// <summary>
    /// Gets or sets the user identifier associated with the consent record.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the type of consent.
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether consent has been given.
    /// </summary>
    public bool ConsentGiven { get; set; }

    /// <summary>
    /// Gets or sets the date when consent was given.
    /// </summary>
    public DateTime ConsentDate { get; set; }

    /// <summary>
    /// Gets or sets the date when consent was withdrawn, if applicable.
    /// </summary>
    public DateTime? WithdrawnDate { get; set; }

    /// <summary>
    /// Gets or sets the purpose for which consent was given.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the legal basis for processing the data.
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;
}

/// <summary>
/// Represents the details of an exported data package provided for data portability.
/// </summary>
public class DataPortabilityResponseDto
{
    /// <summary>
    /// Gets or sets the identifier of the user for whom data was exported.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the format of the exported data.
    /// </summary>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a reference to the exported data package (e.g., a download link or encoded string).
    /// </summary>
    public string DataPackage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the data was exported.
    /// </summary>
    public DateTime ExportedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or system that performed the export.
    /// </summary>
    public string ExportedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the exported data in bytes.
    /// </summary>
    public long DataSize { get; set; }
}

/// <summary>
/// Represents the outcome of a data erasure request, including which data types were erased or retained.
/// </summary>
public class DataErasureResponseDto
{
    /// <summary>
    /// Gets or sets the identifier of the user for whom data was erased.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the erasure was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the collection of data types that were erased.
    /// </summary>
    public IEnumerable<string> ErasedDataTypes { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of data types that were retained.
    /// </summary>
    public IEnumerable<string> RetainedDataTypes { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the reason why certain data was retained.
    /// </summary>
    public string RetentionReason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the data was erased.
    /// </summary>
    public DateTime ErasedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or system that performed the erasure.
    /// </summary>
    public string ErasedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents an audit entry for data processing activities, capturing details about the operation.
/// </summary>
public class DataProcessingAuditDto
{
    /// <summary>
    /// Gets or sets the identifier of the user associated with the processing activity.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the processing activity.
    /// </summary>
    public string ProcessingActivity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the purpose of the processing activity.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the legal basis for the processing.
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the processing occurred.
    /// </summary>
    public DateTime ProcessedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or system that processed the data.
    /// </summary>
    public string ProcessedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data types involved in the processing activity.
    /// </summary>
    public string DataTypes { get; set; } = string.Empty;
}

/// <summary>
/// Represents the results of a privacy impact assessment, including identified risks and mitigation strategies for a data processing activity.
/// </summary>
public class PrivacyImpactAssessmentDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the assessment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the process being assessed.
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a description of the process being assessed.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the risk level assigned to the process (e.g., Low, Medium, High).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of identified risks.
    /// </summary>
    public IEnumerable<string> IdentifiedRisks { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of mitigation actions proposed to address the risks.
    /// </summary>
    public IEnumerable<string> Mitigations { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the date when the assessment was performed.
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or team that performed the assessment.
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents the validation of a lawful basis for processing personal data.
/// </summary>
public class LawfulBasisValidationDto
{
    /// <summary>
    /// Gets or sets the processing activity being validated.
    /// </summary>
    public string ProcessingActivity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lawful basis used for processing.
    /// </summary>
    public string LawfulBasis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the lawful basis is valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the reason behind the validation outcome.
    /// </summary>
    public string ValidationReason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the validation was performed.
    /// </summary>
    public DateTime ValidatedAt { get; set; }
}

/// <summary>
/// Represents a data retention policy outlining how long data should be kept and how it should be disposed of.
/// </summary>
public class DataRetentionPolicyDto
{
    /// <summary>
    /// Gets or sets the type of data the policy applies to.
    /// </summary>
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the period for which the data should be retained.
    /// </summary>
    public TimeSpan RetentionPeriod { get; set; }

    /// <summary>
    /// Gets or sets the reason why the data must be retained.
    /// </summary>
    public string RetentionReason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the method by which the data will be disposed of at the end of the retention period.
    /// </summary>
    public string DisposalMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the policy was created.
    /// </summary>
    public DateTime PolicyCreated { get; set; }

    /// <summary>
    /// Gets or sets the date when the policy was last updated.
    /// </summary>
    public DateTime PolicyUpdated { get; set; }
}

/// <summary>
/// Represents a notification about a data breach, including when and how affected parties were notified.
/// </summary>
public class BreachNotificationDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the breach.
    /// </summary>
    public Guid BreachId { get; set; }

    /// <summary>
    /// Gets or sets the type of notification sent.
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recipient of the notification.
    /// </summary>
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the notification was sent.
    /// </summary>
    public DateTime NotifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the method used to send the notification (e.g., email, phone).
    /// </summary>
    public string NotificationMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the notification (e.g., Sent, Delivered, Failed).
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Represents a report describing the details of a data breach.
/// </summary>
public class DataBreachReportDto
{
    /// <summary>
    /// Gets or sets the type of breach (e.g., hacking, accidental exposure).
    /// </summary>
    public string BreachType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a description of the breach.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the breach was discovered.
    /// </summary>
    public DateTime DiscoveredAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or system that discovered the breach.
    /// </summary>
    public string DiscoveredBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the types of data affected by the breach.
    /// </summary>
    public IEnumerable<string> AffectedDataTypes { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the estimated number of users affected by the breach.
    /// </summary>
    public int EstimatedAffectedUsers { get; set; }

    /// <summary>
    /// Gets or sets the risk level assigned to the breach (e.g., Low, Medium, High).
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the measures taken to contain the breach.
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
}

/// <summary>
/// Represents a record of consent given by a user, including dates and legal details.
/// </summary>
public class ConsentRecordDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the consent record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who gave consent.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the type of consent.
    /// </summary>
    public string ConsentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether consent has been given.
    /// </summary>
    public bool ConsentGiven { get; set; }

    /// <summary>
    /// Gets or sets the date when consent was given.
    /// </summary>
    public DateTime ConsentDate { get; set; }

    /// <summary>
    /// Gets or sets the date when consent was withdrawn, if applicable.
    /// </summary>
    public DateTime? WithdrawnDate { get; set; }

    /// <summary>
    /// Gets or sets the purpose of the consent.
    /// </summary>
    public string Purpose { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the legal basis for the consent.
    /// </summary>
    public string LegalBasis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the method by which consent was recorded (e.g., online form, paper form).
    /// </summary>
    public string ConsentMethod { get; set; } = string.Empty;
}

/// <summary>
/// Represents an escrow arrangement for encryption keys, including creation and expiration details.
/// </summary>
public class KeyEscrowDto
{
    /// <summary>
    /// Gets or sets the identifier of the escrow record.
    /// </summary>
    public string EscrowId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the encryption key placed in escrow.
    /// </summary>
    public string KeyId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reason for the escrow arrangement.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the escrow was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date when the escrow is scheduled to expire.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the person or system that created the escrow.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the escrow arrangement.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets information about authorization required to release the key.
    /// </summary>
    public string AuthorizationRequired { get; set; } = string.Empty;
}

/// <summary>
/// Represents an audit entry for the usage of encryption keys.
/// </summary>
public class KeyUsageAuditDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the audit entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the key that was used.
    /// </summary>
    public string KeyId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operation performed with the key (e.g., encrypt, decrypt).
    /// </summary>
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of entity associated with the key usage (e.g., File, Database).
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the entity involved in the key usage.
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the key was used.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who used the key.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets an error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the IP address from which the key was used.
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
}
