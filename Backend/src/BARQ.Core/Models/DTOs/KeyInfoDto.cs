namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class KeyInfoDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the key.
    /// </summary>
    public string KeyId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the key.
    /// </summary>
    public string KeyName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string KeyType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the key was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the key was last rotated, if applicable.
    /// </summary>
    public DateTime? LastRotatedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the key is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
