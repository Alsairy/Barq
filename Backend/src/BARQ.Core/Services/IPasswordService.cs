using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IPasswordService
{
    Task<PasswordResetResponse> InitiatePasswordResetAsync(string email);
    Task<PasswordResetResponse> ResetPasswordAsync(PasswordResetRequest request);
    Task<PasswordChangeResponse> ChangePasswordAsync(PasswordChangeRequest request);
    Task<PasswordValidationResponse> ValidatePasswordStrengthAsync(string password);
    Task<bool> IsPasswordInHistoryAsync(Guid userId, string password);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
