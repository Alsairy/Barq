using BARQ.Core.Entities;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class RolePermission : BaseEntity
{
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid PermissionId { get; set; }
    
    /// <summary>
    /// </summary>
    public Role Role { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public Permission Permission { get; set; } = null!;
}
