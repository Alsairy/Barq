using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class UserRole : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string RoleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Permissions { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual Role Role { get; set; } = null!;
}
