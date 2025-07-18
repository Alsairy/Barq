namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class EncryptionAuditDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// </summary>
    public string? KeyId { get; set; }

    /// <summary>
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// </summary>
    public string? IPAddress { get; set; }

    /// <summary>
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
}
