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
