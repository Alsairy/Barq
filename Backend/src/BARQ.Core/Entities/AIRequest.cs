using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class AIRequest : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public AIRequestType RequestType { get; set; }

    public AIRequestPriority Priority { get; set; } = AIRequestPriority.Normal;

    public AIRequestStatus Status { get; set; } = AIRequestStatus.Draft;

    public string RequestData { get; set; } = string.Empty;

    public Guid RequesterId { get; set; }
    public virtual User Requester { get; set; } = null!;

    public Guid? AssignedToId { get; set; }
    public virtual User? AssignedTo { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? CompletionNotes { get; set; }

    public virtual ICollection<AIRequestApproval> Approvals { get; set; } = new List<AIRequestApproval>();

    public virtual ICollection<QualityAssessment> QualityAssessments { get; set; } = new List<QualityAssessment>();

    public virtual WorkflowInstance? WorkflowInstance { get; set; }
    public Guid? WorkflowInstanceId { get; set; }
}
