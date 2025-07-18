using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class MfaBackupCode : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// </summary>
    [MaxLength(200)]
    public string? UsedFromIpAddress { get; set; }
}
