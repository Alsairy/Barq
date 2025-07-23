namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a system-defined role that cannot be modified.
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// </summary>
    public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
