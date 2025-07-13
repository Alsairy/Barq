using MediatR;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Application.Queries.Users;

namespace BARQ.Application.Queries.Users;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserProfileService _userProfileService;

    public GetUserProfileQueryHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        return await _userProfileService.GetUserProfileAsync(request.UserId);
    }
}
