using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class PasswordHistory : TenantEntity
{
    [Required]
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;
}
