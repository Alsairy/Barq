using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents an ITSM ticket for external service requests
/// </summary>
public class ITSMTicket : TenantEntity
{
    /// <summary>
    /// Ticket title
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Ticket description
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// ITSM ticket type
    /// </summary>
    public ITSMTicketType TicketType { get; set; }

    /// <summary>
    /// Ticket priority
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// Ticket status
    /// </summary>
    public ITSMTicketStatus Status { get; set; } = ITSMTicketStatus.New;

    /// <summary>
    /// External ticket ID (from ITSM system)
    /// </summary>
    [MaxLength(100)]
    public string? ExternalTicketId { get; set; }

    /// <summary>
    /// ITSM system name (e.g., ServiceNow, Jira Service Management)
    /// </summary>
    [MaxLength(100)]
    public string? ITSMSystem { get; set; }

    /// <summary>
    /// Ticket URL in external system
    /// </summary>
    [MaxLength(500)]
    public string? ExternalUrl { get; set; }

    /// <summary>
    /// Requested by user
    /// </summary>
    public Guid RequestedById { get; set; }
    public virtual User RequestedBy { get; set; } = null!;

    /// <summary>
    /// Assigned to user/group
    /// </summary>
    [MaxLength(200)]
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Assignment group
    /// </summary>
    [MaxLength(200)]
    public string? AssignmentGroup { get; set; }

    /// <summary>
    /// Ticket category
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Ticket subcategory
    /// </summary>
    [MaxLength(100)]
    public string? Subcategory { get; set; }

    /// <summary>
    /// Business justification
    /// </summary>
    [MaxLength(1000)]
    public string? BusinessJustification { get; set; }

    /// <summary>
    /// Expected completion date
    /// </summary>
    public DateTime? ExpectedCompletionDate { get; set; }

    /// <summary>
    /// Actual completion date
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// SLA due date
    /// </summary>
    public DateTime? SLADueDate { get; set; }

    /// <summary>
    /// Resolution notes
    /// </summary>
    public string? ResolutionNotes { get; set; }

    /// <summary>
    /// Closure code
    /// </summary>
    [MaxLength(50)]
    public string? ClosureCode { get; set; }

    /// <summary>
    /// Estimated cost
    /// </summary>
    public decimal? EstimatedCost { get; set; }

    /// <summary>
    /// Actual cost
    /// </summary>
    public decimal? ActualCost { get; set; }

    /// <summary>
    /// Approval required
    /// </summary>
    public bool RequiresApproval { get; set; } = false;

    /// <summary>
    /// Approval status
    /// </summary>
    public ReviewStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// Approver user ID
    /// </summary>
    public Guid? ApproverId { get; set; }
    public virtual User? Approver { get; set; }

    /// <summary>
    /// Approval comments
    /// </summary>
    public string? ApprovalComments { get; set; }

    /// <summary>
    /// Ticket metadata as JSON
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Project this ticket is related to
    /// </summary>
    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Sprint this ticket is related to
    /// </summary>
    public Guid? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// AI task that triggered this ticket
    /// </summary>
    public Guid? AITaskId { get; set; }
    public virtual AITask? AITask { get; set; }

    /// <summary>
    /// Workflow instance that triggered this ticket
    /// </summary>
    public Guid? WorkflowInstanceId { get; set; }
    public virtual WorkflowInstance? WorkflowInstance { get; set; }

    /// <summary>
    /// Ticket updates and comments
    /// </summary>
    public virtual ICollection<ITSMTicketUpdate> Updates { get; set; } = new List<ITSMTicketUpdate>();
}

/// <summary>
/// Represents an update or comment on an ITSM ticket
/// </summary>
public class ITSMTicketUpdate : TenantEntity
{
    /// <summary>
    /// Update content
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Update type
    /// </summary>
    public ITSMUpdateType UpdateType { get; set; } = ITSMUpdateType.Comment;

    /// <summary>
    /// Update source (internal or external)
    /// </summary>
    public ITSMUpdateSource Source { get; set; } = ITSMUpdateSource.Internal;

    /// <summary>
    /// User who created the update
    /// </summary>
    public Guid? UpdatedByUserId { get; set; }
    public virtual User? UpdatedBy { get; set; }

    /// <summary>
    /// External user name (if from ITSM system)
    /// </summary>
    [MaxLength(200)]
    public string? ExternalUserName { get; set; }

    /// <summary>
    /// Is public update (visible to requestor)
    /// </summary>
    public bool IsPublic { get; set; } = true;

    /// <summary>
    /// ITSM ticket this update belongs to
    /// </summary>
    public Guid ITSMTicketId { get; set; }
    public virtual ITSMTicket ITSMTicket { get; set; } = null!;
}

