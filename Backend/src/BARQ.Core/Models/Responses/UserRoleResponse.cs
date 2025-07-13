using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class UserRoleResponse : BaseResponse
{
    public UserRoleDto? UserRole { get; set; }
}

public class RoleResponse : BaseResponse
{
    public RoleDto? Role { get; set; }
}

public class RoleValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public string Permission { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<string> ValidationErrors { get; set; } = new List<string>();
}
