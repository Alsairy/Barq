using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class MfaBackupCode : TenantEntity
{
    [Required]
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    public bool IsUsed { get; set; } = false;

    public DateTime? UsedAt { get; set; }

    [MaxLength(200)]
    public string? UsedFromIpAddress { get; set; }
}
