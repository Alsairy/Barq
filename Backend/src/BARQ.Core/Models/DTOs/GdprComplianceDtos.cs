namespace BARQ.Core.Models.DTOs;

public class DataSubjectRequestDto
{
    public Guid UserId { get; set; }
    public string RequestType { get; set; } = string.Empty; // Access, Rectification, Erasure, Portability, Restriction
    public string Description { get; set; } = string.Empty;
    public string IdentityVerification { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
}

public class DataSubjectRightsResponseDto
{
    public Guid RequestId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class ConsentUpdateRequestDto
{
    public Guid UserId { get; set; }
    public string ConsentType { get; set; } = string.Empty;
    public bool ConsentGiven { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public DateTime ConsentDate { get; set; }
    public string LegalBasis { get; set; } = string.Empty;
}

public class ConsentManagementResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string ConsentId { get; set; } = string.Empty;
}

public class ConsentStatusDto
{
    public Guid UserId { get; set; }
    public string ConsentType { get; set; } = string.Empty;
    public bool ConsentGiven { get; set; }
    public DateTime ConsentDate { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string LegalBasis { get; set; } = string.Empty;
}

public class DataPortabilityResponseDto
{
    public Guid UserId { get; set; }
    public string Format { get; set; } = string.Empty;
    public string DataPackage { get; set; } = string.Empty;
    public DateTime ExportedAt { get; set; }
    public string ExportedBy { get; set; } = string.Empty;
    public long DataSize { get; set; }
}

public class DataErasureResponseDto
{
    public Guid UserId { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> ErasedDataTypes { get; set; } = new List<string>();
    public IEnumerable<string> RetainedDataTypes { get; set; } = new List<string>();
    public string RetentionReason { get; set; } = string.Empty;
    public DateTime ErasedAt { get; set; }
    public string ErasedBy { get; set; } = string.Empty;
}

public class DataProcessingAuditDto
{
    public Guid UserId { get; set; }
    public string ProcessingActivity { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string LegalBasis { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } = string.Empty;
    public string DataTypes { get; set; } = string.Empty;
}

public class PrivacyImpactAssessmentDto
{
    public Guid Id { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public IEnumerable<string> IdentifiedRisks { get; set; } = new List<string>();
    public IEnumerable<string> Mitigations { get; set; } = new List<string>();
    public DateTime AssessmentDate { get; set; }
    public string AssessedBy { get; set; } = string.Empty;
}

public class LawfulBasisValidationDto
{
    public string ProcessingActivity { get; set; } = string.Empty;
    public string LawfulBasis { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public string ValidationReason { get; set; } = string.Empty;
    public DateTime ValidatedAt { get; set; }
}

public class DataRetentionPolicyDto
{
    public string DataType { get; set; } = string.Empty;
    public TimeSpan RetentionPeriod { get; set; }
    public string RetentionReason { get; set; } = string.Empty;
    public string DisposalMethod { get; set; } = string.Empty;
    public DateTime PolicyCreated { get; set; }
    public DateTime PolicyUpdated { get; set; }
}

public class BreachNotificationDto
{
    public Guid BreachId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public DateTime NotifiedAt { get; set; }
    public string NotificationMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class DataBreachReportDto
{
    public string BreachType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DiscoveredAt { get; set; }
    public string DiscoveredBy { get; set; } = string.Empty;
    public IEnumerable<string> AffectedDataTypes { get; set; } = new List<string>();
    public int EstimatedAffectedUsers { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string ContainmentMeasures { get; set; } = string.Empty;
}

public class ConsentRecordDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ConsentType { get; set; } = string.Empty;
    public bool ConsentGiven { get; set; }
    public DateTime ConsentDate { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string LegalBasis { get; set; } = string.Empty;
    public string ConsentMethod { get; set; } = string.Empty;
}

public class KeyEscrowDto
{
    public string EscrowId { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string AuthorizationRequired { get; set; } = string.Empty;
}

public class KeyUsageAuditDto
{
    public Guid Id { get; set; }
    public string KeyId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}
