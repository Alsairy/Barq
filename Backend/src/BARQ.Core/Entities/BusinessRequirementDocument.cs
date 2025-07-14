using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class BusinessRequirementDocument : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Version { get; set; } = "1.0";
    
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    public DateTime? ApprovedAt { get; set; }
    
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
    
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; } = null!;
    
    public Guid? ApproverId { get; set; }
    public virtual User? Approver { get; set; }
}
