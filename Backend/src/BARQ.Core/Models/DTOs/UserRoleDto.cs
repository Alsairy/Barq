namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for user role assignment information
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// Unique identifier for the user role assignment
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Identifier of the user assigned to the role
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Identifier of the role assigned to the user
    /// </summary>
    public Guid RoleId { get; set; }
    /// <summary>
    /// Name of the role assigned to the user
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
    /// <summary>
    /// Description of the role assigned to the user
    /// </summary>
    public string RoleDescription { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the user who assigned the role
    /// </summary>
    public Guid? AssignedBy { get; set; }
    /// <summary>
    /// Name of the user who assigned the role
    /// </summary>
    public string AssignedByName { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the role was assigned
    /// </summary>
    public DateTime AssignedAt { get; set; }
    /// <summary>
    /// Date and time when the role assignment expires
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    /// <summary>
    /// Indicates whether the role assignment is currently active
    /// </summary>
    public bool IsActive { get; set; }
}
