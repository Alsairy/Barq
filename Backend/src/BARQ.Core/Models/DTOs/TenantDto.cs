namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing tenant information (important-comment)
/// </summary>
public class TenantDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the tenant (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the tenant description (important-comment)
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the tenant domain
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the tenant is active (important-comment)
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant creation date (important-comment)
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the number of users in the tenant (important-comment)
    /// </summary>
    public int UserCount { get; set; }
    
    /// <summary>
    /// Gets or sets the number of projects in the tenant (important-comment)
    /// </summary>
    public int ProjectCount { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant subscription plan (important-comment)
    /// </summary>
    public string SubscriptionPlan { get; set; } = string.Empty;
}
