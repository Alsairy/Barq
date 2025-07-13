namespace BARQ.Core.Models.Responses;

public class LogoutResponse : BaseResponse
{
    public bool LoggedOut { get; set; }
}

public class SessionValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public Guid? UserId { get; set; }
}

public class AccountLockoutResponse : BaseResponse
{
    public bool IsLockedOut { get; set; }
    public DateTime? LockoutExpiresAt { get; set; }
    public int FailedAttempts { get; set; }
    public int MaxAttempts { get; set; }
}

public class MfaSetupResponse : BaseResponse
{
    public string? QrCodeUrl { get; set; }
    public string? ManualEntryKey { get; set; }
    public string[]? BackupCodes { get; set; }
}

public class MfaVerificationResponse : BaseResponse
{
    public bool IsVerified { get; set; }
    public string? AccessToken { get; set; }
}

public class BackupCodesResponse : BaseResponse
{
    public string[] BackupCodes { get; set; } = Array.Empty<string>();
}

public class MfaDisableResponse : BaseResponse
{
    public bool IsDisabled { get; set; }
}

public class MfaRecoveryResponse : BaseResponse
{
    public bool RecoveryInitiated { get; set; }
    public string? RecoveryToken { get; set; }
}

public class PasswordResetResponse : BaseResponse
{
    public bool ResetInitiated { get; set; }
    public string? ResetToken { get; set; }
}

public class PasswordChangeResponse : BaseResponse
{
    public bool PasswordChanged { get; set; }
}

public class PasswordValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public int StrengthScore { get; set; }
    public ICollection<string> ValidationMessages { get; set; } = new List<string>();
}
