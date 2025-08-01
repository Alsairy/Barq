namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class TenantDto
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
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public int UserCount { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProjectCount { get; set; }
    
    /// <summary>
    /// </summary>
    public string SubscriptionPlan { get; set; } = string.Empty;
}
