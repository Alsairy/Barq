using MediatR;
using BARQ.Core.Services;
using BARQ.Core.Models.Responses;
using BARQ.Application.Commands.Authentication;

namespace BARQ.Application.Commands.Authentication;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.AuthenticateAsync(request.Request);
    }
}
