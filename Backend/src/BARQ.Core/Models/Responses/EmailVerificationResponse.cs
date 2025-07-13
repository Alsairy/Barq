namespace BARQ.Core.Models.Responses;

public class EmailVerificationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
}
