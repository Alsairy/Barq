using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class ProjectMember : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// </summary>
    public DateTime? LeftAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// </summary>
    public decimal? AllocationPercentage { get; set; } = 100;
    
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual Project Project { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual User User { get; set; } = null!
}
