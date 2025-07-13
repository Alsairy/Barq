namespace BARQ.Core.Models.Requests;

public class AssignRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid AssignedBy { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class RemoveRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid RemovedBy { get; set; }
    public bool ForceRemove { get; set; } = false;
}

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid TenantId { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}

public class UpdateRoleRequest
{
    public Guid RoleId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Guid>? PermissionIds { get; set; }
    public bool AllowSystemRoleUpdate { get; set; } = false;
}
