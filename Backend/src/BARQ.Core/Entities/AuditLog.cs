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
}
