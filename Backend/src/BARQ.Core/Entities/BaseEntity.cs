using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// Base entity class that provides common properties for all entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the entity was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID of the user who created the entity
    /// </summary>
    public Guid? CreatedById { get; set; }

    /// <summary>
    /// ID of the user who last updated the entity
    /// </summary>
    public Guid? UpdatedById { get; set; }

    /// <summary>
    /// Indicates if the entity is soft deleted
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Date and time when the entity was soft deleted
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// ID of the user who deleted the entity
    /// </summary>
    public Guid? DeletedById { get; set; }

    /// <summary>
    /// Version number for optimistic concurrency control
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}

/// <summary>
/// Base entity class for tenant-aware entities
/// </summary>
public abstract class TenantEntity : BaseEntity
{
    /// <summary>
    /// Tenant ID for multi-tenancy support
    /// </summary>
    [Required]
    public Guid TenantId { get; set; }
}

