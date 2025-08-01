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
/// </summary>
public class PrivacyImpactAssessmentDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> IdentifiedRisks { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> Mitigations { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public DateTime AssessmentDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string AssessedBy { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class LawfulBasisValidationDto
{
    /// <summary>
    /// </summary>
    public string ProcessingActivity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string LawfulBasis { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public string ValidationReason { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime ValidatedAt { get; set; }
}

/// <summary>
/// </summary>
public class DataRetentionPolicyDto
{
    /// <summary>
    /// </summary>
    public string DataType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public TimeSpan RetentionPeriod { get; set; }
    /// <summary>
    /// </summary>
    public string RetentionReason { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string DisposalMethod { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime PolicyCreated { get; set; }
    /// <summary>
    /// </summary>
    public DateTime PolicyUpdated { get; set; }
}

/// <summary>
/// </summary>
public class BreachNotificationDto
{
    /// <summary>
    /// </summary>
    public Guid BreachId { get; set; }
    /// <summary>
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Recipient { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime NotifiedAt { get; set; }
    /// <summary>
    /// </summary>
    public string NotificationMethod { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class DataBreachReportDto
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
    public IEnumerable<string> AffectedDataTypes { get; set; } = new List<string>();
    /// <summary>
    /// </summary>
    public int EstimatedAffectedUsers { get; set; }
    /// <summary>
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string ContainmentMeasures { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ConsentRecordDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
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
    /// <summary>
    /// </summary>
    public string ConsentMethod { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class KeyEscrowDto
{
    /// <summary>
    /// </summary>
    public string EscrowId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    /// <summary>
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string AuthorizationRequired { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class KeyUsageAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Operation { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string EntityType { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public Guid EntityId { get; set; }
    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
}
