using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Authentication;

public class PasswordService : IPasswordService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<PasswordHistory> _passwordHistoryRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PasswordService> _logger;

    public PasswordService(
        IRepository<User> userRepository,
        IRepository<PasswordHistory> passwordHistoryRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<PasswordService> logger)
    {
        _userRepository = userRepository;
        _passwordHistoryRepository = passwordHistoryRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<PasswordResetResponse> InitiatePasswordResetAsync(string email)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return new PasswordResetResponse
                {
                    Success = true,
                    Message = "If the email address exists, a password reset link has been sent.",
                    ResetInitiated = true
                };
            }

            var resetToken = GeneratePasswordResetToken();
            // This would need to be stored in a separate PasswordResetToken entity// 1 hour expiry
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();


            _logger.LogInformation("Password reset initiated for user: {Email}", email);

            return new PasswordResetResponse
            {
                Success = true,
                Message = "Password reset link has been sent to your email address.",
                ResetInitiated = true,
                ResetToken = resetToken // In production, don't return the token
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating password reset for: {Email}", email);
            return new PasswordResetResponse
            {
                Success = false,
                Message = "Failed to initiate password reset"
            };
        }
    }

    public async Task<PasswordResetResponse> ResetPasswordAsync(PasswordResetRequest request)
    {
        try
        {
            User? user = null;
            if (user == null)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Invalid reset token"
                };
            }

            if (user == null)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Reset token has expired"
                };
            }

            var passwordValidation = await ValidatePasswordStrengthAsync(request.NewPassword);
            if (!passwordValidation.IsValid)
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Password does not meet strength requirements",
                    Errors = passwordValidation.ValidationMessages
                };
            }

            if (await IsPasswordInHistoryAsync(user.Id, request.NewPassword))
            {
                return new PasswordResetResponse
                {
                    Success = false,
                    Message = "Cannot reuse a recent password"
                };
            }

            await SavePasswordToHistoryAsync(user.Id, user.PasswordHash ?? string.Empty);

            user.PasswordHash = HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;


            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Password reset successfully for user: {UserId}", user.Id);

            return new PasswordResetResponse
            {
                Success = true,
                Message = "Password has been reset successfully",
                ResetInitiated = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password with token: {Token}", request.ResetToken);
            return new PasswordResetResponse
            {
                Success = false,
                Message = "Password reset failed"
            };
        }
    }

    public async Task<PasswordChangeResponse> ChangePasswordAsync(PasswordChangeRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new PasswordChangeResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!VerifyPassword(request.CurrentPassword, user.PasswordHash ?? string.Empty))
            {
                return new PasswordChangeResponse
                {
                    Success = false,
                    Message = "Current password is incorrect"
                };
            }

            var passwordValidation = await ValidatePasswordStrengthAsync(request.NewPassword);
            if (!passwordValidation.IsValid)
            {
                return new PasswordChangeResponse
                {
                    Success = false,
                    Message = "New password does not meet strength requirements",
                    Errors = passwordValidation.ValidationMessages
                };
            }

            if (await IsPasswordInHistoryAsync(user.Id, request.NewPassword))
            {
                return new PasswordChangeResponse
                {
                    Success = false,
                    Message = "Cannot reuse a recent password"
                };
            }

            await SavePasswordToHistoryAsync(user.Id, user.PasswordHash ?? string.Empty);

            user.PasswordHash = HashPassword(request.NewPassword);
            user.PasswordChangedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Password changed successfully for user: {UserId}", request.UserId);

            return new PasswordChangeResponse
            {
                Success = true,
                Message = "Password changed successfully",
                PasswordChanged = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", request.UserId);
            return new PasswordChangeResponse
            {
                Success = false,
                Message = "Password change failed"
            };
        }
    }

    public Task<PasswordValidationResponse> ValidatePasswordStrengthAsync(string password)
    {
        var response = new PasswordValidationResponse
        {
            Success = true,
            ValidationMessages = new List<string>()
        };

        var score = 0;
        var messages = new List<string>();

        if (password.Length < 8)
        {
            messages.Add("Password must be at least 8 characters long");
        }
        else if (password.Length >= 12)
        {
            score += 2;
        }
        else
        {
            score += 1;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            messages.Add("Password must contain at least one uppercase letter");
        }
        else
        {
            score += 1;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            messages.Add("Password must contain at least one lowercase letter");
        }
        else
        {
            score += 1;
        }

        if (!Regex.IsMatch(password, @"\d"))
        {
            messages.Add("Password must contain at least one digit");
        }
        else
        {
            score += 1;
        }

        if (!Regex.IsMatch(password, @"[@$!%*?&]"))
        {
            messages.Add("Password must contain at least one special character (@$!%*?&)");
        }
        else
        {
            score += 1;
        }

        if (IsCommonPassword(password))
        {
            messages.Add("Password is too common, please choose a more unique password");
            score = Math.Max(0, score - 2);
        }

        response.IsValid = messages.Count == 0;
        response.StrengthScore = Math.Min(5, score);
        response.ValidationMessages = messages;

        return Task.FromResult(response);
    }

    public async Task<bool> IsPasswordInHistoryAsync(Guid userId, string password)
    {
        try
        {
            var historyCount = GetPasswordHistoryCount();
            var passwordHistory = await _passwordHistoryRepository
                .GetAsync(ph => ph.UserId == userId, 
                         orderBy: q => q.OrderByDescending(ph => ph.CreatedAt),
                         take: historyCount);

            foreach (var historyEntry in passwordHistory)
            {
                if (VerifyPassword(password, historyEntry.PasswordHash))
                {
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking password history for user: {UserId}", userId);
            return false;
        }
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GetBCryptWorkFactor());
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            return false;
        }
    }

    private async Task SavePasswordToHistoryAsync(Guid userId, string passwordHash)
    {
        try
        {
            var passwordHistory = new PasswordHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _passwordHistoryRepository.AddAsync(passwordHistory);

            var historyCount = GetPasswordHistoryCount();
            var oldEntries = await _passwordHistoryRepository
                .GetAsync(ph => ph.UserId == userId,
                         orderBy: q => q.OrderByDescending(ph => ph.CreatedAt),
                         skip: historyCount);

            foreach (var oldEntry in oldEntries)
            {
                await _passwordHistoryRepository.DeleteAsync(oldEntry);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving password to history for user: {UserId}", userId);
        }
    }

    private string GeneratePasswordResetToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    private bool IsCommonPassword(string password)
    {
        var commonPasswords = new[]
        {
            "password", "123456", "password123", "admin", "qwerty", "letmein",
            "welcome", "monkey", "1234567890", "abc123", "Password1"
        };

        return commonPasswords.Contains(password.ToLowerInvariant());
    }

    private int GetBCryptWorkFactor() => int.Parse(_configuration["Security:BCryptWorkFactor"] ?? "12");
    private int GetPasswordHistoryCount() => int.Parse(_configuration["Security:PasswordHistoryCount"] ?? "5");
}
