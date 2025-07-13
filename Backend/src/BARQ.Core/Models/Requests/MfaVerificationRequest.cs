using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

public class MfaVerificationRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string MfaCode { get; set; } = string.Empty;

    public string? DeviceId { get; set; }
}

public class BackupCodeVerificationRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string BackupCode { get; set; } = string.Empty;
}

public class MfaRecoveryRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? AlternateEmail { get; set; }
}
