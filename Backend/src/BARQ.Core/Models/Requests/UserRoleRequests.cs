namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class AssignRoleRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid AssignedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// </summary>
public class RemoveRoleRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid RemovedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public bool ForceRemove { get; set; } = false;
}

/// <summary>
/// </summary>
public class CreateRoleRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid>? PermissionIds { get; set; }
}

/// <summary>
/// </summary>
public class UpdateRoleRequest
{
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid>? PermissionIds { get; set; }
    
    /// <summary>
    /// </summary>
    public bool AllowSystemRoleUpdate { get; set; } = false;
}
