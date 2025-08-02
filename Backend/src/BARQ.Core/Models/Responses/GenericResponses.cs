namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class LogoutResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool LoggedOut { get; set; }
}

/// <summary>
/// </summary>
public class SessionValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? UserId { get; set; }
}

/// <summary>
/// </summary>
public class AccountLockoutResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsLockedOut { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? LockoutExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public int FailedAttempts { get; set; }
    
    /// <summary>
    /// </summary>
    public int MaxAttempts { get; set; }
}

/// <summary>
/// </summary>
public class MfaSetupResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string? QrCodeUrl { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ManualEntryKey { get; set; }
    
    /// <summary>
    /// </summary>
    public string[]? BackupCodes { get; set; }
}

/// <summary>
/// </summary>
public class MfaVerificationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsVerified { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AccessToken { get; set; }
}

/// <summary>
/// </summary>
public class BackupCodesResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string[] BackupCodes { get; set; } = Array.Empty<string>();
}

/// <summary>
/// </summary>
public class MfaDisableResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsDisabled { get; set; }
}

/// <summary>
/// </summary>
public class MfaRecoveryResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool RecoveryInitiated { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RecoveryToken { get; set; }
}

/// <summary>
/// </summary>
public class PasswordResetResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool ResetInitiated { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ResetToken { get; set; }
}

/// <summary>
/// </summary>
public class PasswordChangeResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool PasswordChanged { get; set; }
}

/// <summary>
/// </summary>
public class PasswordValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public int StrengthScore { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> ValidationMessages { get; set; } = new List<string>();
}
