using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using QRCoder;
using OtpNet;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Authentication;

public class MultiFactorAuthService : IMultiFactorAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<MfaBackupCode> _backupCodeRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MultiFactorAuthService> _logger;

    public MultiFactorAuthService(
        IRepository<User> userRepository,
        IRepository<MfaBackupCode> backupCodeRepository,
        IPasswordService passwordService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<MultiFactorAuthService> logger)
    {
        _userRepository = userRepository;
        _backupCodeRepository = backupCodeRepository;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MfaSetupResponse> SetupMfaAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new MfaSetupResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.TwoFactorEnabled)
            {
                return new MfaSetupResponse
                {
                    Success = false,
                    Message = "MFA is already enabled for this user"
                };
            }

            var secretKey = GenerateSecretKey();
            var issuer = GetMfaIssuer();
            var accountName = user.Email;

            var totpUrl = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(accountName)}?secret={secretKey}&issuer={Uri.EscapeDataString(issuer)}";

            var qrCodeUrl = GenerateQrCode(totpUrl);

            var backupCodes = GenerateBackupCodes();

            user.TwoFactorEnabled = false; // Will be enabled after verification
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            foreach (var code in backupCodes)
            {
                var backupCodeEntity = new MfaBackupCode
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Code = HashBackupCode(code),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _backupCodeRepository.AddAsync(backupCodeEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("MFA setup initiated for user: {UserId}", userId);

            return new MfaSetupResponse
            {
                Success = true,
                Message = "MFA setup initiated. Please scan the QR code with your authenticator app.",
                QrCodeUrl = qrCodeUrl,
                ManualEntryKey = secretKey,
                BackupCodes = backupCodes.ToArray()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up MFA for user: {UserId}", userId);
            return new MfaSetupResponse
            {
                Success = false,
                Message = "MFA setup failed"
            };
        }
    }

    public async Task<MfaVerificationResponse> VerifyMfaSetupAsync(MfaVerificationRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (string.IsNullOrEmpty(user.MfaSecretKey))
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "MFA setup not initiated"
                };
            }

            var isValid = VerifyTotpCode(user.MfaSecretKey, request.MfaCode);
            if (!isValid)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "Invalid MFA code"
                };
            }

            user.MfaEnabled = true;
            user.MfaEnabledAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("MFA enabled successfully for user: {UserId}", request.UserId);

            return new MfaVerificationResponse
            {
                Success = true,
                Message = "MFA enabled successfully",
                IsVerified = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying MFA setup for user: {UserId}", request.UserId);
            return new MfaVerificationResponse
            {
                Success = false,
                Message = "MFA verification failed"
            };
        }
    }

    public async Task<MfaVerificationResponse> VerifyMfaCodeAsync(MfaVerificationRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this user"
                };
            }

            var isValid = VerifyTotpCode("temp-secret", request.MfaCode);
            if (!isValid)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "Invalid MFA code"
                };
            }

            _logger.LogInformation("MFA code verified successfully for user: {UserId}", request.UserId);

            return new MfaVerificationResponse
            {
                Success = true,
                Message = "MFA code verified successfully",
                IsVerified = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying MFA code for user: {UserId}", request.UserId);
            return new MfaVerificationResponse
            {
                Success = false,
                Message = "MFA verification failed"
            };
        }
    }

    public async Task<BackupCodesResponse> GenerateNewBackupCodesAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new BackupCodesResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new BackupCodesResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this user"
                };
            }

            var existingCodes = await _backupCodeRepository.FindAsync(bc => bc.UserId == userId);
            foreach (var code in existingCodes)
            {
                await _backupCodeRepository.DeleteAsync(code);
            }

            var newBackupCodes = GenerateBackupCodes();

            foreach (var code in newBackupCodes)
            {
                var backupCodeEntity = new MfaBackupCode
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Code = HashBackupCode(code),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _backupCodeRepository.AddAsync(backupCodeEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New backup codes generated for user: {UserId}", userId);

            return new BackupCodesResponse
            {
                Success = true,
                Message = "New backup codes generated successfully",
                BackupCodes = newBackupCodes
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating new backup codes for user: {UserId}", userId);
            return new BackupCodesResponse
            {
                Success = false,
                Message = "Failed to generate new backup codes"
            };
        }
    }

    public async Task<MfaVerificationResponse> VerifyBackupCodeAsync(BackupCodeVerificationRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this user"
                };
            }

            var hashedCode = HashBackupCode(request.BackupCode);
            var backupCodes = await _backupCodeRepository.FindAsync(
                bc => bc.UserId == request.UserId && bc.Code == hashedCode && !bc.IsUsed);
            var backupCode = backupCodes.FirstOrDefault();

            if (backupCode == null)
            {
                return new MfaVerificationResponse
                {
                    Success = false,
                    Message = "Invalid or already used backup code"
                };
            }

            backupCode.IsUsed = true;
            backupCode.UsedAt = DateTime.UtcNow;
            await _backupCodeRepository.UpdateAsync(backupCode);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Backup code verified successfully for user: {UserId}", request.UserId);

            return new MfaVerificationResponse
            {
                Success = true,
                Message = "Backup code verified successfully",
                IsVerified = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying backup code for user: {UserId}", request.UserId);
            return new MfaVerificationResponse
            {
                Success = false,
                Message = "Backup code verification failed"
            };
        }
    }

    public async Task<MfaDisableResponse> DisableMfaAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new MfaDisableResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new MfaDisableResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this user"
                };
            }

            user.TwoFactorEnabled = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            var backupCodes = await _backupCodeRepository.GetAsync(bc => bc.UserId == userId);
            foreach (var code in backupCodes)
            {
                await _backupCodeRepository.DeleteAsync(code);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("MFA disabled successfully for user: {UserId}", userId);

            return new MfaDisableResponse
            {
                Success = true,
                Message = "MFA disabled successfully",
                IsDisabled = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling MFA for user: {UserId}", userId);
            return new MfaDisableResponse
            {
                Success = false,
                Message = "Failed to disable MFA"
            };
        }
    }

    public async Task<MfaRecoveryResponse> InitiateMfaRecoveryAsync(MfaRecoveryRequest request)
    {
        try
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant());
            if (user == null)
            {
                return new MfaRecoveryResponse
                {
                    Success = true,
                    Message = "If the email address exists, recovery instructions have been sent.",
                    RecoveryInitiated = true
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new MfaRecoveryResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this account"
                };
            }

            var recoveryToken = GenerateRecoveryToken();
            user.MfaRecoveryToken = recoveryToken;
            user.MfaRecoveryTokenExpiry = DateTime.UtcNow.AddHours(1);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();


            _logger.LogInformation("MFA recovery initiated for user: {Email}", request.Email);

            return new MfaRecoveryResponse
            {
                Success = true,
                Message = "MFA recovery instructions have been sent to your email address.",
                RecoveryInitiated = true,
                RecoveryToken = recoveryToken // In production, don't return the token
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating MFA recovery for: {Email}", request.Email);
            return new MfaRecoveryResponse
            {
                Success = false,
                Message = "MFA recovery failed"
            };
        }
    }

    private string GenerateSecretKey()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(key);
    }

    private string[] GenerateBackupCodes()
    {
        var codes = new string[8];
        using var rng = RandomNumberGenerator.Create();
        
        for (int i = 0; i < codes.Length; i++)
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var code = BitConverter.ToUInt32(bytes, 0) % 100000000;
            codes[i] = code.ToString("D8");
        }

        return codes;
    }

    private string HashBackupCode(string code)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code + GetBackupCodeSalt()));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyTotpCode(string secretKey, string code)
    {
        try
        {
            var keyBytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(keyBytes);
            return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }
        catch
        {
            return false;
        }
    }

    private string GenerateQrCode(string totpUrl)
    {
        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);
            return $"data:image/png;base64,{Convert.ToBase64String(qrCodeBytes)}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code");
            return string.Empty;
        }
    }

    private string GenerateRecoveryToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    private string GetMfaIssuer() => _configuration["Mfa:Issuer"] ?? "BARQ";
    private string GetBackupCodeSalt() => _configuration["Mfa:BackupCodeSalt"] ?? "barq-backup-salt";

    public async Task<BackupCodesResponse> GenerateBackupCodesAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new BackupCodesResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var existingCodes = await _backupCodeRepository.GetAsync(bc => bc.UserId == userId);
            foreach (var code in existingCodes)
            {
                await _backupCodeRepository.DeleteAsync(code);
            }

            var backupCodes = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var code = GenerateBackupCode();
                backupCodes.Add(code);

                var backupCodeEntity = new MfaBackupCode
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Code = HashBackupCode(code),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _backupCodeRepository.AddAsync(backupCodeEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Generated backup codes for user: {UserId}", userId);

            return new BackupCodesResponse
            {
                Success = true,
                Message = "Backup codes generated successfully",
                BackupCodes = backupCodes.ToArray()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating backup codes for user: {UserId}", userId);
            return new BackupCodesResponse
            {
                Success = false,
                Message = "Failed to generate backup codes"
            };
        }
    }

    public async Task<MfaDisableResponse> DisableMfaAsync(Guid userId, string currentPassword)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new MfaDisableResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!_passwordService.VerifyPassword(currentPassword, user.PasswordHash ?? string.Empty))
            {
                return new MfaDisableResponse
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }

            if (!user.TwoFactorEnabled)
            {
                return new MfaDisableResponse
                {
                    Success = false,
                    Message = "MFA is not enabled for this user"
                };
            }

            user.TwoFactorEnabled = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            var backupCodes = await _backupCodeRepository.GetAsync(bc => bc.UserId == userId);
            foreach (var code in backupCodes)
            {
                await _backupCodeRepository.DeleteAsync(code);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("MFA disabled for user: {UserId}", userId);

            return new MfaDisableResponse
            {
                Success = true,
                Message = "MFA has been disabled successfully",
                IsDisabled = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling MFA for user: {UserId}", userId);
            return new MfaDisableResponse
            {
                Success = false,
                Message = "Failed to disable MFA"
            };
        }
    }

    public async Task<bool> IsMfaEnabledAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.TwoFactorEnabled ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking MFA status for user: {UserId}", userId);
            return false;
        }
    }

    private string GenerateBackupCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[8];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "").Substring(0, 8);
    }
}
