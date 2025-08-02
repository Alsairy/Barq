using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? MfaCode { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RememberMe { get; set; }
    
    /// <summary>
    /// </summary>
    public string? DeviceId { get; set; }
}
