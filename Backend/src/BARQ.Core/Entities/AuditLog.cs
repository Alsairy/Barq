using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class AuditLog : TenantEntity
{
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;
    
    public Guid EntityId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    
    [MaxLength(500)]
    public string? IPAddress { get; set; }
    
    [MaxLength(1000)]
    public string? UserAgent { get; set; }
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? SessionId { get; set; }
    public string? CorrelationId { get; set; }
    public string? AdditionalData { get; set; }
    public string? Source { get; set; }
    public string? Category { get; set; }
    public string? Severity { get; set; }
    public string? Description { get; set; }
    public bool? IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? Duration { get; set; }
    
    [MaxLength(50)]
    public string? ComplianceFramework { get; set; }
    
    [MaxLength(100)]
    public string? ComplianceEventType { get; set; }
    
    [MaxLength(50)]
    public string? DataClassification { get; set; }
    
    [MaxLength(100)]
    public string? LegalBasis { get; set; }
    
    [MaxLength(50)]
    public string? RetentionPeriod { get; set; }
    
    public DateTime? RetentionExpiryDate { get; set; }
    public bool IsPersonalData { get; set; }
    public bool IsSensitiveData { get; set; }
    
    [MaxLength(100)]
    public string? ConsentId { get; set; }
    
    [MaxLength(200)]
    public string? ProcessingPurpose { get; set; }
    
    [MaxLength(100)]
    public string? DataSubjectId { get; set; }
    
    [MaxLength(50)]
    public string? ComplianceStatus { get; set; }
    
    [MaxLength(200)]
    public string? RegulatoryRequirement { get; set; }
    
    public bool RequiresNotification { get; set; }
    public DateTime? NotificationDeadline { get; set; }
    
    [MaxLength(500)]
    public string? DigitalSignature { get; set; }
    
    [MaxLength(100)]
    public string? IntegrityHash { get; set; }
    
    public bool IsTamperProof { get; set; }
    
    [MaxLength(500)]
    public string? ArchiveLocation { get; set; }
    
    public string? ComplianceNotes { get; set; }
}
