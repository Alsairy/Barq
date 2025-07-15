using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class AuditLog : TenantEntity
{
    [Required]
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string EntityName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid EntityId { get; set; }
    
    [Required]
    [MaxLength(50)]
    /// <summary>
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? OldValues { get; set; }
    /// <summary>
    /// </summary>
    public string? NewValues { get; set; }
    
    [MaxLength(500)]
    /// <summary>
    /// </summary>
    public string? IPAddress { get; set; }
    
    [MaxLength(1000)]
    /// <summary>
    /// </summary>
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
    
    [MaxLength(50)]
    /// <summary>
    /// </summary>
    public string? ComplianceFramework { get; set; }
    
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string? ComplianceEventType { get; set; }
    
    [MaxLength(50)]
    /// <summary>
    /// </summary>
    public string? DataClassification { get; set; }
    
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string? LegalBasis { get; set; }
    
    [MaxLength(50)]
    /// <summary>
    /// </summary>
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
    
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string? ConsentId { get; set; }
    
    [MaxLength(200)]
    /// <summary>
    /// </summary>
    public string? ProcessingPurpose { get; set; }
    
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string? DataSubjectId { get; set; }
    
    [MaxLength(50)]
    /// <summary>
    /// </summary>
    public string? ComplianceStatus { get; set; }
    
    [MaxLength(200)]
    /// <summary>
    /// </summary>
    public string? RegulatoryRequirement { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresNotification { get; set; }
    /// <summary>
    /// </summary>
    public DateTime? NotificationDeadline { get; set; }
    
    [MaxLength(500)]
    /// <summary>
    /// </summary>
    public string? DigitalSignature { get; set; }
    
    [MaxLength(100)]
    /// <summary>
    /// </summary>
    public string? IntegrityHash { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsTamperProof { get; set; }
    
    [MaxLength(500)]
    /// <summary>
    /// </summary>
    public string? ArchiveLocation { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ComplianceNotes { get; set; }
}
