using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// Request model for user logout operations
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to logout (important-comment)
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the refresh token to invalidate during logout (important-comment)
    /// </summary>
    public string? RefreshToken { get; set; }
}

/// <summary>
/// Request model for refreshing authentication tokens
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Gets or sets the refresh token used to obtain new access tokens (important-comment)
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Request model for initiating password reset process
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Gets or sets the email address for password reset (important-comment)
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Request model for completing password reset with token
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// Gets or sets the password reset token (important-comment)
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the new password
    /// </summary>
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password confirmation (important-comment)
    /// </summary>
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Request model for changing user password
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user changing password (important-comment)
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the current password for verification (important-comment)
    /// </summary>
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the new password
    /// </summary>
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password confirmation (important-comment)
    /// </summary>
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Request model for setting up multi-factor authentication
/// </summary>
public class MfaSetupRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user setting up MFA (important-comment)
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the authenticator key for MFA setup (important-comment)
    /// </summary>
    public string? AuthenticatorKey { get; set; }
}

/// <summary>
/// Request model for disabling multi-factor authentication
/// </summary>
public class MfaDisableRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user disabling MFA (important-comment)
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the password for verification (important-comment)
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the verification code for MFA disable (important-comment)
    /// </summary>
    public string? VerificationCode { get; set; }
}

/// <summary>
/// Request model for regenerating MFA backup codes
/// </summary>
public class RegenerateBackupCodesRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user regenerating backup codes (important-comment)
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the password for verification (important-comment)
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
