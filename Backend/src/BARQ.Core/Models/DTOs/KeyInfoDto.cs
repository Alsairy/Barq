namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class KeyInfoDto
{
    /// <summary>
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string KeyName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string KeyType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? LastRotatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
