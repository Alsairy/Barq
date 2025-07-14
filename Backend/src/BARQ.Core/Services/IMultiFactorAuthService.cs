using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IMultiFactorAuthService
{
    Task<MfaSetupResponse> SetupMfaAsync(Guid userId);
    Task<MfaVerificationResponse> VerifyMfaCodeAsync(MfaVerificationRequest request);
    Task<BackupCodesResponse> GenerateBackupCodesAsync(Guid userId);
    Task<MfaVerificationResponse> VerifyBackupCodeAsync(BackupCodeVerificationRequest request);
    Task<MfaDisableResponse> DisableMfaAsync(Guid userId, string currentPassword);
    Task<MfaRecoveryResponse> InitiateMfaRecoveryAsync(MfaRecoveryRequest request);
    Task<bool> IsMfaEnabledAsync(Guid userId);
    Task<MfaSetupResponse> SetupHardwareTokenAsync(Guid userId, string tokenType);
    Task<MfaSetupResponse> SetupBiometricAuthAsync(Guid userId, string biometricType);
}
