namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserActivationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsActivated { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ActivatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> Errors { get; set; } = new List<string>();
}
