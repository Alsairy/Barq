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
        Console.WriteLine($">>> LoginCommandHandler.Handle called with email: {request.Request?.Email}");
        
        if (request.Request == null)
        {
            Console.WriteLine(">>> LoginCommandHandler.Handle: request.Request is null");
            return new AuthenticationResponse
            {
                Success = false,
                Message = "Invalid login request"
            };
        }
        
        var result = await _authenticationService.AuthenticateAsync(request.Request);
        Console.WriteLine($">>> LoginCommandHandler.Handle result: Success={result.Success}, AccessToken length={result.AccessToken?.Length ?? 0}");
        return result;
    }
}
