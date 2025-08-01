using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RefreshToken { get; set; }
}

/// <summary>
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class MfaSetupRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AuthenticatorKey { get; set; }
}

/// <summary>
/// </summary>
public class MfaDisableRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? VerificationCode { get; set; }
}

/// <summary>
/// </summary>
public class RegenerateBackupCodesRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
