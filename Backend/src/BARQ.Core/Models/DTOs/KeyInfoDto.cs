namespace BARQ.Core.Models.DTOs;

public class KeyInfoDto
{
    public string KeyId { get; set; } = string.Empty;
    public string KeyName { get; set; } = string.Empty;
    public string KeyType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastRotatedAt { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class KeyEscrowDto
{
    public string EscrowId { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string AuthorizationRequired { get; set; } = string.Empty;
}

public class KeyUsageAuditDto
{
    public Guid Id { get; set; }
    public string KeyId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
