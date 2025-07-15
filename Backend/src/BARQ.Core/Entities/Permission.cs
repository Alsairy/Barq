using BARQ.Core.Entities;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Resource { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsSystemPermission { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
