namespace BARQ.Core.Models.Responses;

public class UserRegistrationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public bool RequiresEmailVerification { get; set; }
    public string? VerificationToken { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
}
