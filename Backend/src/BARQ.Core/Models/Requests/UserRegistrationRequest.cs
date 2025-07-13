using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

public class UserRegistrationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    public Guid? OrganizationId { get; set; }
    public string? InvitationToken { get; set; }
    public bool AcceptTerms { get; set; }
}
