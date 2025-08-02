using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// </summary>
    Task<PasswordResetResponse> InitiatePasswordResetAsync(string email);

    /// <summary>
    /// </summary>
    Task<PasswordResetResponse> ResetPasswordAsync(PasswordResetRequest request);

    /// <summary>
    /// </summary>
    Task<PasswordChangeResponse> ChangePasswordAsync(PasswordChangeRequest request);

    /// <summary>
    /// </summary>
    Task<PasswordValidationResponse> ValidatePasswordStrengthAsync(string password);

    /// <summary>
    /// </summary>
    Task<bool> IsPasswordInHistoryAsync(Guid userId, string password);

    /// <summary>
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// </summary>
    bool VerifyPassword(string password, string hashedPassword);
}
