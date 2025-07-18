using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class Role : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// </summary>
    public bool IsSystemRole { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// </summary>
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    /// <summary>
    /// </summary>
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
