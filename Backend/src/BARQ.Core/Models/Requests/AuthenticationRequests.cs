using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

public class LogoutRequest
{
    public Guid UserId { get; set; }
    public string? RefreshToken { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class MfaSetupRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    public string? AuthenticatorKey { get; set; }
}

public class MfaDisableRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public string? VerificationCode { get; set; }
}

public class RegenerateBackupCodesRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Password { get; set; } = string.Empty;
}
