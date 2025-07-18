using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class PasswordHistory : TenantEntity
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
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;
}
