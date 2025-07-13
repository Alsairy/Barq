using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class UserRole : TenantEntity
{
    [Required]
    [MaxLength(100)]
    public string RoleName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public string? Permissions { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
}
