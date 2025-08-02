namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string RoleDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid? AssignedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string AssignedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime AssignedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
}
