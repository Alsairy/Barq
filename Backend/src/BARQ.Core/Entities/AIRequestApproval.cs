using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>Represents an approval step for an AI request, including approver, level, type, status, and delegation details.</summary> (important-comment)
public class AIRequestApproval : BaseEntity
{
    /// <summary>Identifier of the associated AI request.</summary> (important-comment)
    public Guid AIRequestId { get; set; }

    /// <summary>Navigation property of the associated AI request.</summary> (important-comment)
    public virtual AIRequest AIRequest { get; set; } = null!;

    /// <summary>Identifier of the approver user.</summary> (important-comment)
    public Guid ApproverId { get; set; }

    /// <summary>Navigation property of the approver user.</summary> (important-comment)
    public virtual User Approver { get; set; } = null!;

    /// <summary>Approval level (e.g., L1, L2).</summary> (important-comment)
    public ApprovalLevel Level { get; set; }

    /// <summary>Type of approval (e.g., technical, business).</summary> (important-comment)
    public ApprovalType Type { get; set; }

    /// <summary>Current approval status.</summary> (important-comment)
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

    /// <summary>Timestamp when the approval was granted, if any.</summary> (important-comment)
    public DateTime? ApprovedAt { get; set; }

    /// <summary>Optional approver comments.</summary> (important-comment)
    [MaxLength(1000)]
    public string? Comments { get; set; }

    /// <summary>Optional reason provided when the approval is rejected.</summary> (important-comment)
    [MaxLength(1000)]
    public string? RejectionReason { get; set; }

    /// <summary>Indicates whether this approval is required for the request to proceed.</summary> (important-comment)
    public bool IsRequired { get; set; } = true;

    /// <summary>Order of this approval in the overall approval sequence.</summary> (important-comment)
    public int Order { get; set; }

    /// <summary>Due date by which this approval should be completed.</summary> (important-comment)
    public DateTime? DueDate { get; set; }

    /// <summary>Identifier of a user to whom this approval was delegated, if any.</summary> (important-comment)
    public Guid? DelegatedToId { get; set; }

    /// <summary>Navigation property of the user to whom this approval was delegated, if any.</summary> (important-comment)
    public virtual User? DelegatedTo { get; set; }

    /// <summary>Identifier of a user who delegated this approval, if any.</summary> (important-comment)
    public Guid? DelegatedFromId { get; set; }

    /// <summary>Navigation property of the user who delegated this approval, if any.</summary> (important-comment)
    public virtual User? DelegatedFrom { get; set; }

    /// <summary>Timestamp when the approval was delegated, if any.</summary> (important-comment)
    public DateTime? DelegatedAt { get; set; }

    /// <summary>Optional reason describing the delegation.</summary> (important-comment)
    [MaxLength(500)]
    public string? DelegationReason { get; set; }
}
