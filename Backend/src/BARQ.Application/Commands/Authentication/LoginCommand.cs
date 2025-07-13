using MediatR;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Application.Commands.Authentication;

public record LoginCommand(LoginRequest Request) : IRequest<AuthenticationResponse>;
