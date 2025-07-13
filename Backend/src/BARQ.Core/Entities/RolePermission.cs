using BARQ.Core.Entities;

namespace BARQ.Core.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
