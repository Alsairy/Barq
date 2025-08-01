using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class PasswordResetRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string ResetToken { get; set; } = string.Empty;

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
public class PasswordChangeRequest
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
