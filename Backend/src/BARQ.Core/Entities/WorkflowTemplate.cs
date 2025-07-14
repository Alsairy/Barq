using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a workflow template for approval processes
/// </summary>
public class WorkflowTemplate : TenantEntity
{
    /// <summary>
    /// Template name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Template description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Workflow type
    /// </summary>
    public WorkflowType WorkflowType { get; set; }

    /// <summary>
    /// Template version
    /// </summary>
    [MaxLength(20)]
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// Workflow definition as JSON
    /// </summary>
    public string WorkflowDefinition { get; set; } = string.Empty;

    /// <summary>
    /// Approval steps as JSON array
    /// </summary>
    public string ApprovalSteps { get; set; } = string.Empty;

    /// <summary>
    /// SLA configuration as JSON
    /// </summary>
    public string? SLAConfiguration { get; set; }

    /// <summary>
    /// </summary>
    public int? SlaHours { get; set; }

    /// <summary>
    /// Escalation rules as JSON
    /// </summary>
    public string? EscalationRules { get; set; }

    /// <summary>
    /// Notification settings as JSON
    /// </summary>
    public string? NotificationSettings { get; set; }

    /// <summary>
    /// Template is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Template is default for this workflow type
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Organization this template belongs to
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// Workflow instances created from this template
    /// </summary>
    public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
}

