namespace BARQ.Core.Models.DTOs;

public class EncryptionAuditDto
{
    public Guid Id { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? KeyId { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
