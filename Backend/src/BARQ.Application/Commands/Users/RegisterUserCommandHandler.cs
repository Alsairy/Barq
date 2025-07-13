using MediatR;
using BARQ.Core.Services;
using BARQ.Core.Models.Responses;
using BARQ.Application.Commands.Users;

namespace BARQ.Application.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserRegistrationResponse>
{
    private readonly IUserRegistrationService _userRegistrationService;

    public RegisterUserCommandHandler(IUserRegistrationService userRegistrationService)
    {
        _userRegistrationService = userRegistrationService;
    }

    public async Task<UserRegistrationResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _userRegistrationService.RegisterUserAsync(request.Request);
    }
}
