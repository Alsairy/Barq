namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class PermissionDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
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
    /// </summary>
    public bool IsSystemPermission { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
