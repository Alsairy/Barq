using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IUserRoleService
{
    Task<UserRoleResponse> AssignRoleAsync(AssignRoleRequest request);
    Task<UserRoleResponse> RemoveRoleAsync(RemoveRoleRequest request);
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId);
    Task<RoleValidationResponse> ValidateUserPermissionAsync(Guid userId, string permission);
    Task<IEnumerable<RoleDto>> GetAvailableRolesAsync();
    Task<RoleDto> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleDto> UpdateRoleAsync(UpdateRoleRequest request);
}
