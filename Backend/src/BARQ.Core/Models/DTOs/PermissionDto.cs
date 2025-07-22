namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class PermissionDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the permission.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
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
    /// Gets or sets a value indicating whether this is a system-defined permission that cannot be modified.
    /// </summary>
    public bool IsSystemPermission { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the permission was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
