namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for role information and permissions
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Unique identifier for the role
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the role
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description of the role and its purpose
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Indicates whether this is a system-defined role (important-comment)
    /// </summary>
    public bool IsSystemRole { get; set; }
    /// <summary>
    /// Date and time when the role was created (important-comment)
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Collection of permissions assigned to this role
    /// </summary>
    public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
