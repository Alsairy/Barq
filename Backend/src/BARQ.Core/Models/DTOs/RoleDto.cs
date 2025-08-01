namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class RoleDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsSystemRole { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
