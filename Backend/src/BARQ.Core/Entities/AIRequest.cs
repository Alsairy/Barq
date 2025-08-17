using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>Represents a user-submitted AI request within a tenant, including metadata, priority, status, and workflow linkage.</summary>
public class AIRequest : TenantEntity
{
    /// <summary>Short, human-readable title of the request.</summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Detailed description of the request.</summary>
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>The type or category of the AI request.</summary>
    public AIRequestType RequestType { get; set; }

    /// <summary>Priority level for handling this request.</summary>
    public AIRequestPriority Priority { get; set; } = AIRequestPriority.Normal;

    /// <summary>Current lifecycle status of the request.</summary>
    public AIRequestStatus Status { get; set; } = AIRequestStatus.Draft;

    /// <summary>Arbitrary request payload or serialized parameters.</summary>
    public string RequestData { get; set; } = string.Empty;

    /// <summary>Identifier of the user who created the request.</summary>
    public Guid RequesterId { get; set; }

    /// <summary>Navigation property of the requesting user.</summary>
    public virtual User Requester { get; set; } = null!;

    /// <summary>Identifier of the user assigned to handle this request, if any.</summary>
    public Guid? AssignedToId { get; set; }

    /// <summary>Navigation property of the assigned user, if any.</summary>
    public virtual User? AssignedTo { get; set; }

    /// <summary>Desired due date for completing the request.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Timestamp when the request was completed, if any.</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Notes or comments added upon completion.</summary>
    public string? CompletionNotes { get; set; }

    /// <summary>Approval records associated with the request.</summary>
    public virtual ICollection<AIRequestApproval> Approvals { get; set; } = new List<AIRequestApproval>();

    /// <summary>Quality assessments performed on the request output.</summary>
    public virtual ICollection<QualityAssessment> QualityAssessments { get; set; } = new List<QualityAssessment>();

    /// <summary>Workflow instance that tracks request processing.</summary>
    public virtual WorkflowInstance? WorkflowInstance { get; set; }

    /// <summary>Identifier of the workflow instance, if any.</summary>
    public Guid? WorkflowInstanceId { get; set; }
}
