namespace BARQ.Core.Models.Responses;

public class AuthenticationResponse
{
    public bool Success { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool RequiresMfa { get; set; }
    public string? MfaToken { get; set; }
    public string? Message { get; set; }
    public Guid? UserId { get; set; }
    public string? UserEmail { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
}
