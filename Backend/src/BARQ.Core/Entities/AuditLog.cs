using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class AuditLog : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid EntityId { get; set; }
    
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? OldValues { get; set; }
    
    /// <summary>
    /// </summary>
    public string? NewValues { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(500)]
    public string? IPAddress { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(1000)]
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? CorrelationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AdditionalData { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Source { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Severity { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? IsSuccessful { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan? Duration { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(50)]
    public string? ComplianceFramework { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(100)]
    public string? ComplianceEventType { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(50)]
    public string? DataClassification { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(100)]
    public string? LegalBasis { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(50)]
    public string? RetentionPeriod { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? RetentionExpiryDate { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsPersonalData { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsSensitiveData { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(100)]
    public string? ConsentId { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(200)]
    public string? ProcessingPurpose { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(100)]
    public string? DataSubjectId { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(50)]
    public string? ComplianceStatus { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(200)]
    public string? RegulatoryRequirement { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresNotification { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? NotificationDeadline { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(500)]
    public string? DigitalSignature { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(100)]
    public string? IntegrityHash { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsTamperProof { get; set; }
    
    /// <summary>
    /// </summary>
    [MaxLength(500)]
    public string? ArchiveLocation { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ComplianceNotes { get; set; }
}
