using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class MfaVerificationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public string MfaCode { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? DeviceId { get; set; }
}

/// <summary>
/// </summary>
public class BackupCodeVerificationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public string BackupCode { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class MfaRecoveryRequest
{
    /// <summary>
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? AlternateEmail { get; set; }
}
