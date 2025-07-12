using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class ProjectMember : TenantEntity
{
    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}
