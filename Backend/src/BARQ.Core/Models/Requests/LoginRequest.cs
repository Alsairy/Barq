using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? MfaCode { get; set; }
    public bool RememberMe { get; set; }
    public string? DeviceId { get; set; }
}
