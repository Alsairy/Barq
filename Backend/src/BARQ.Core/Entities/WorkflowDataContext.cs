using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class WorkflowDataContext : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [MaxLength(50)]
    public string Scope { get; set; } = "workflow";

    /// <summary>
    /// </summary>
    public string Data { get; set; } = "{}";

    /// <summary>
    /// </summary>
    public string? DataSchema { get; set; }

    /// <summary>
    /// </summary>
    public string? EncryptionKeyId { get; set; }

    /// <summary>
    /// </summary>
    public bool IsEncrypted { get; set; } = false;

    /// <summary>
    /// </summary>
    public string? AccessPermissions { get; set; }

    /// <summary>
    /// </summary>
    public string? ValidationRules { get; set; }

    /// <summary>
    /// </summary>
    public string? TransformationRules { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// </summary>
    public Guid? WorkflowInstanceId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowInstance? WorkflowInstance { get; set; }

    /// <summary>
    /// </summary>
    public Guid? WorkflowStepId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowStep? WorkflowStep { get; set; }

    /// <summary>
    /// </summary>
    public Guid? ParentContextId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowDataContext? ParentContext { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<WorkflowDataContext> ChildContexts { get; set; } = new List<WorkflowDataContext>();
}
