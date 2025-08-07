namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for system permissions and access control information (important-comment)
/// </summary>
public class PermissionDto
{
    /// <summary>
    /// Unique identifier for the permission
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the permission
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description of what the permission allows
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Resource that the permission applies to
    /// </summary>
    public string Resource { get; set; } = string.Empty;
    /// <summary>
    /// Action that the permission allows on the resource
    /// </summary>
    public string Action { get; set; } = string.Empty;
    /// <summary>
    /// Indicates whether this is a system-level permission
    /// </summary>
    public bool IsSystemPermission { get; set; }
    /// <summary>
    /// Date and time when the permission was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
