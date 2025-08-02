using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserRoleResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public UserRoleDto? UserRole { get; set; }
}

/// <summary>
/// </summary>
public class RoleResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public RoleDto? Role { get; set; }
}

/// <summary>
/// </summary>
public class RoleValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public string Permission { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Resource { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new List<string>();
}
