namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object containing encryption key information and metadata
/// </summary>
public class KeyInfoDto
{
    /// <summary>
    /// Unique identifier for the encryption key
    /// </summary>
    public string KeyId { get; set; } = string.Empty;
    /// <summary>
    /// Human-readable name for the encryption key
    /// </summary>
    public string KeyName { get; set; } = string.Empty;
    /// <summary>
    /// Type classification of the encryption key
    /// </summary>
    public string KeyType { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the key was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date and time when the key was last rotated
    /// </summary>
    public DateTime? LastRotatedAt { get; set; }
    /// <summary>
    /// Indicates whether the key is currently active
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Current status of the encryption key
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
