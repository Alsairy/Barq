using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IUserRoleService
{
    /// <summary>
    /// </summary>
    Task<UserRoleResponse> AssignRoleAsync(AssignRoleRequest request);

    /// <summary>
    /// </summary>
    Task<UserRoleResponse> RemoveRoleAsync(RemoveRoleRequest request);

    /// <summary>
    /// </summary>
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<RoleValidationResponse> ValidateUserPermissionAsync(Guid userId, string permission);

    /// <summary>
    /// </summary>
    Task<IEnumerable<RoleDto>> GetAvailableRolesAsync();

    /// <summary>
    /// </summary>
    Task<RoleDto> CreateRoleAsync(CreateRoleRequest request);

    /// <summary>
    /// </summary>
    Task<RoleDto> UpdateRoleAsync(UpdateRoleRequest request);
}
