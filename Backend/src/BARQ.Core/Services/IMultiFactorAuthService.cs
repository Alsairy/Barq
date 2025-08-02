using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IMultiFactorAuthService
{
    /// <summary>
    /// </summary>
    Task<MfaSetupResponse> SetupMfaAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<MfaVerificationResponse> VerifyMfaCodeAsync(MfaVerificationRequest request);

    /// <summary>
    /// </summary>
    Task<BackupCodesResponse> GenerateBackupCodesAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<MfaVerificationResponse> VerifyBackupCodeAsync(BackupCodeVerificationRequest request);

    /// <summary>
    /// </summary>
    Task<MfaDisableResponse> DisableMfaAsync(Guid userId, string currentPassword);

    /// <summary>
    /// </summary>
    Task<MfaRecoveryResponse> InitiateMfaRecoveryAsync(MfaRecoveryRequest request);

    /// <summary>
    /// </summary>
    Task<bool> IsMfaEnabledAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<MfaSetupResponse> SetupHardwareTokenAsync(Guid userId, string tokenType);

    /// <summary>
    /// </summary>
    Task<MfaSetupResponse> SetupBiometricAuthAsync(Guid userId, string biometricType);
}
