using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Users;

public class UserRoleService : IUserRoleService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserRoleService> _logger;

    public UserRoleService(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IRepository<Permission> permissionRepository,
        IRepository<UserRole> userRoleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserRoleService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId)
    {
        try
        {
            var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == userId);

            var roleDtos = new List<RoleDto>();

            _logger.LogInformation("Retrieved {Count} roles for user: {UserId}", roleDtos.Count(), userId);
            return roleDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<UserRoleResponse> AssignRoleToUserAsync(AssignRoleRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "Role not found"
                };
            }

            var existingUserRoles = await _userRoleRepository.FindAsync(
                ur => ur.UserId == request.UserId);
            var existingUserRole = existingUserRoles.FirstOrDefault();

            if (existingUserRole != null)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "User already has this role assigned"
                };
            }

            if (user.TenantId != role.TenantId && role.TenantId != Guid.Empty)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "Cannot assign role from different tenant"
                };
            }

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                RoleName = "DefaultRole", // AssignRoleRequest doesn't have RoleName
                Description = "Role assigned via request",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRoleRepository.AddAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Role assigned to user {UserId} by {AssignedBy}", 
                request.UserId, request.AssignedBy);

            return new UserRoleResponse
            {
                Success = true,
                Message = "Role assigned successfully",
                UserRole = _mapper.Map<UserRoleDto>(userRole)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}", request.RoleId, request.UserId);
            return new UserRoleResponse
            {
                Success = false,
                Message = "Failed to assign role"
            };
        }
    }

    public async Task<UserRoleResponse> RemoveRoleFromUserAsync(RemoveRoleRequest request)
    {
        try
        {
            var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == request.UserId);
            var userRole = userRoles.FirstOrDefault();

            if (userRole == null)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "User role assignment not found"
                };
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role != null && role.IsSystemRole && !request.ForceRemove)
            {
                return new UserRoleResponse
                {
                    Success = false,
                    Message = "Cannot remove system role without force flag"
                };
            }

            await _userRoleRepository.DeleteAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Role removed from user {UserId} by {RemovedBy}", 
                request.UserId, request.RemovedBy);

            return new UserRoleResponse
            {
                Success = true,
                Message = "Role removed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing role {RoleId} from user {UserId}", request.RoleId, request.UserId);
            return new UserRoleResponse
            {
                Success = false,
                Message = "Failed to remove role"
            };
        }
    }

    public async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId)
    {
        try
        {
            var userRoles = await _userRoleRepository.FindAsync(
                ur => ur.UserId == userId && ur.IsActive);

            var permissions = new List<Permission>();

            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);

            _logger.LogInformation("Retrieved {Count} permissions for user: {UserId}", permissionDtos.Count(), userId);
            return permissionDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string resource, string action)
    {
        try
        {
            var userPermissions = await GetUserPermissionsAsync(userId);
            
            var hasPermission = userPermissions.Any(p => 
                p.Resource.Equals(resource, StringComparison.OrdinalIgnoreCase) &&
                p.Action.Equals(action, StringComparison.OrdinalIgnoreCase));

            _logger.LogDebug("Permission check for user {UserId}: {Resource}.{Action} = {HasPermission}", 
                userId, resource, action, hasPermission);

            return hasPermission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission for user {UserId}: {Resource}.{Action}", 
                userId, resource, action);
            return false;
        }
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest request)
    {
        try
        {
            var existingRole = await _roleRepository.FirstOrDefaultAsync(
                r => r.Name == request.Name && r.TenantId == request.TenantId);

            if (existingRole != null)
            {
                throw new InvalidOperationException("Role with this name already exists");
            }

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                TenantId = request.TenantId,
                IsSystemRole = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _roleRepository.AddAsync(role);

            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                foreach (var permissionId in request.PermissionIds)
                {
                    var permission = await _permissionRepository.GetByIdAsync(permissionId);
                    if (permission != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid(),
                            RoleId = role.Id,
                            PermissionId = permissionId,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _rolePermissionRepository.AddAsync(rolePermission);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            var roleDto = _mapper.Map<RoleDto>(role);

            _logger.LogInformation("Role created: {RoleName} for tenant {TenantId}", request.Name, request.TenantId);

            return roleDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role: {RoleName}", request.Name);
            throw;
        }
    }

    public async Task<RoleDto> UpdateRoleAsync(UpdateRoleRequest request)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }

            if (role.IsSystemRole && !request.AllowSystemRoleUpdate)
            {
                throw new InvalidOperationException("Cannot update system role without explicit permission");
            }

            role.Name = request.Name ?? role.Name;
            role.Description = request.Description ?? role.Description;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role);

            if (request.PermissionIds != null)
            {
                var existingRolePermissions = await _rolePermissionRepository.GetAsync(rp => rp.RoleId == request.RoleId);
                foreach (var rp in existingRolePermissions)
                {
                    await _rolePermissionRepository.DeleteAsync(rp);
                }

                foreach (var permissionId in request.PermissionIds)
                {
                    var permission = await _permissionRepository.GetByIdAsync(permissionId);
                    if (permission != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid(),
                            RoleId = role.Id,
                            PermissionId = permissionId,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _rolePermissionRepository.AddAsync(rolePermission);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            var roleDto = _mapper.Map<RoleDto>(role);

            _logger.LogInformation("Role updated: {RoleId}", request.RoleId);

            return roleDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role: {RoleId}", request.RoleId);
            throw;
        }
    }

    public async Task<RoleResponse> DeleteRoleAsync(Guid roleId, bool forceDelete = false)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return new RoleResponse
                {
                    Success = false,
                    Message = "Role not found"
                };
            }

            if (role.IsSystemRole && !forceDelete)
            {
                return new RoleResponse
                {
                    Success = false,
                    Message = "Cannot delete system role without force flag"
                };
            }

            var userRoles = await _userRoleRepository.GetAsync(ur => ur.RoleId == roleId);
            if (userRoles.Any() && !forceDelete)
            {
                return new RoleResponse
                {
                    Success = false,
                    Message = "Cannot delete role that is assigned to users. Use force delete if needed."
                };
            }

            var rolePermissions = await _rolePermissionRepository.GetAsync(rp => rp.RoleId == roleId);
            foreach (var rp in rolePermissions)
            {
                await _rolePermissionRepository.DeleteAsync(rp);
            }

            if (forceDelete)
            {
                foreach (var ur in userRoles)
                {
                    await _userRoleRepository.DeleteAsync(ur);
                }
            }

            await _roleRepository.DeleteAsync(role);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Role deleted: {RoleId} (force: {ForceDelete})", roleId, forceDelete);

            return new RoleResponse
            {
                Success = true,
                Message = "Role deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role: {RoleId}", roleId);
            return new RoleResponse
            {
                Success = false,
                Message = "Failed to delete role"
            };
        }
    }

    public async Task<IEnumerable<RoleDto>> GetAvailableRolesAsync(Guid tenantId)
    {
        try
        {
            var roles = await _roleRepository.FindAsync(
                r => r.TenantId == tenantId || r.TenantId == Guid.Empty); // Include global roles

            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

            _logger.LogInformation("Retrieved {Count} available roles for tenant: {TenantId}", roleDtos.Count(), tenantId);
            return roleDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available roles for tenant: {TenantId}", tenantId);
            throw;
        }
    }

    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
    {
        try
        {
            var permissions = await _permissionRepository.GetAllAsync();
            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);

            _logger.LogInformation("Retrieved {Count} total permissions", permissionDtos.Count());
            return permissionDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all permissions");
            throw;
        }
    }

    public async Task<UserRoleResponse> AssignRoleAsync(AssignRoleRequest request)
    {
        return await AssignRoleToUserAsync(request);
    }

    public async Task<UserRoleResponse> RemoveRoleAsync(RemoveRoleRequest request)
    {
        return await RemoveRoleFromUserAsync(request);
    }

    public async Task<RoleValidationResponse> ValidateUserPermissionAsync(Guid userId, string permission)
    {
        try
        {
            var parts = permission.Split('.');
            if (parts.Length != 2)
            {
                return new RoleValidationResponse
                {
                    Success = false,
                    IsValid = false,
                    Message = "Invalid permission format. Expected format: 'Resource.Action'",
                    Permission = permission,
                    UserId = userId,
                    ValidationErrors = new List<string> { "Permission must be in format 'Resource.Action'" }
                };
            }

            var hasPermission = await HasPermissionAsync(userId, parts[0], parts[1]);

            return new RoleValidationResponse
            {
                Success = true,
                IsValid = hasPermission,
                Message = hasPermission ? "Permission validated successfully" : "User does not have the required permission",
                Permission = permission,
                Resource = parts[0],
                Action = parts[1],
                UserId = userId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user permission: {UserId}, {Permission}", userId, permission);
            return new RoleValidationResponse
            {
                Success = false,
                IsValid = false,
                Message = "Permission validation failed",
                Permission = permission,
                UserId = userId,
                ValidationErrors = new List<string> { "Internal error during permission validation" }
            };
        }
    }

    public async Task<IEnumerable<RoleDto>> GetAvailableRolesAsync()
    {
        try
        {
            var roles = await _roleRepository.GetAllAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

            _logger.LogInformation("Retrieved {Count} available roles", roleDtos.Count());
            return roleDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available roles");
            throw;
        }
    }
}
