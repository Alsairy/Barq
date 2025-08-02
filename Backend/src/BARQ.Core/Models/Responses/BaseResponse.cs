namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public abstract class BaseResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public ICollection<string> Errors { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
