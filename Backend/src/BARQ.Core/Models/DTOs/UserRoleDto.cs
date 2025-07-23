namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user role assignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user to whom the role is assigned.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the role being assigned.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the role being assigned.
    /// </summary>
    public string RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who assigned this role, if available.
    /// </summary>
    public Guid? AssignedBy { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who assigned this role.
    /// </summary>
    public string AssignedByName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the role was assigned.
    /// </summary>
    public DateTime AssignedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the role assignment expires, if applicable.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the role assignment is currently active.
    /// </summary>
    public bool IsActive { get; set; }
}
