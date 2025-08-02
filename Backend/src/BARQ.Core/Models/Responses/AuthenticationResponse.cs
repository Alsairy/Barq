namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AccessToken { get; set; }
    
    /// <summary>
    /// </summary>
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresMfa { get; set; }
    
    /// <summary>
    /// </summary>
    public string? MfaToken { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? UserEmail { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> Roles { get; set; } = new List<string>();
}
