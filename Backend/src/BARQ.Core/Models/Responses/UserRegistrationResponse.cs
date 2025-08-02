namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserRegistrationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresEmailVerification { get; set; }
    
    /// <summary>
    /// </summary>
    public string? VerificationToken { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> Errors { get; set; } = new List<string>();
}
