namespace BARQ.Core.Models.DTOs;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
