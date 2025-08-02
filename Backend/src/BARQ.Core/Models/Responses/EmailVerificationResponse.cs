namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class EmailVerificationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsVerified { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? VerifiedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> Errors { get; set; } = new List<string>();
}
