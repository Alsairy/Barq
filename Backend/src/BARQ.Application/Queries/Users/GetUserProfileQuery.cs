using MediatR;
using BARQ.Core.Models.DTOs;

namespace BARQ.Application.Queries.Users;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
