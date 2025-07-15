using AutoMapper;
using BARQ.Core.Entities;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;

namespace BARQ.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.TenantName, opt => opt.Ignore());

        CreateMap<UserRegistrationRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.OrganizationId ?? Guid.Empty));

        CreateMap<Organization, OrganizationDto>()
            .ForMember(dest => dest.UserCount, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectCount, opt => opt.Ignore());

        CreateMap<Role, RoleDto>();
        CreateMap<Permission, PermissionDto>();
    }
}
