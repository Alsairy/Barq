using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class BusinessRequirementDocument : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [MaxLength(20)]
    public string Version { get; set; } = "1.0";
    
    /// <summary>
    /// </summary>
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// </summary>
    public DateTime? ApprovedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual Project Project { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public Guid AuthorId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual User Author { get; set; } = null!;
    
    /// <summary>
    /// </summary>
    public Guid? ApproverId { get; set; }
    
    /// <summary>
    /// </summary>
    public virtual User? Approver { get; set; }
}
