using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

public class AIRequestApproval : BaseEntity
{
    public Guid AIRequestId { get; set; }
    public virtual AIRequest AIRequest { get; set; } = null!;

    public Guid ApproverId { get; set; }
    public virtual User Approver { get; set; } = null!;

    public ApprovalLevel Level { get; set; }

    public ApprovalType Type { get; set; }

    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

    public DateTime? ApprovedAt { get; set; }

    [MaxLength(1000)]
    public string? Comments { get; set; }

    [MaxLength(1000)]
    public string? RejectionReason { get; set; }

    public bool IsRequired { get; set; } = true;

    public int Order { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? DelegatedToId { get; set; }
    public virtual User? DelegatedTo { get; set; }

    public Guid? DelegatedFromId { get; set; }
    public virtual User? DelegatedFrom { get; set; }

    public DateTime? DelegatedAt { get; set; }

    [MaxLength(500)]
    public string? DelegationReason { get; set; }
}
