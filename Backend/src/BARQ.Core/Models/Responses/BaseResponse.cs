namespace BARQ.Core.Models.Responses;

public abstract class BaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ICollection<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
