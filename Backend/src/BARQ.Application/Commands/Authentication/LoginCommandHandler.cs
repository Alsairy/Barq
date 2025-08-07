using MediatR;
using BARQ.Core.Services;
using BARQ.Core.Models.Responses;
using BARQ.Application.Commands.Authentication;
using Microsoft.Extensions.Logging;

namespace BARQ.Application.Commands.Authentication;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthenticationService authenticationService, ILogger<LoginCommandHandler> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("LoginCommandHandler: Processing login request for email: {Email}", request.Request.Email);
            var result = await _authenticationService.AuthenticateAsync(request.Request);
            _logger.LogInformation("LoginCommandHandler: Authentication result - Success: {Success}, AccessToken length: {TokenLength}", 
                result.Success, result.AccessToken?.Length ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoginCommandHandler: Error processing login request for email: {Email}", request.Request.Email);
            return new AuthenticationResponse
            {
                Success = false,
                Message = "Authentication failed due to internal error"
            };
        }
    }
}
