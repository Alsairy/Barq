using MediatR;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Application.Commands.Users;

public record RegisterUserCommand(UserRegistrationRequest Request) : IRequest<UserRegistrationResponse>;
