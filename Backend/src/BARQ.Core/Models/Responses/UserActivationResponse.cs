namespace BARQ.Core.Models.Responses;

public class UserActivationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsActivated { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
}
