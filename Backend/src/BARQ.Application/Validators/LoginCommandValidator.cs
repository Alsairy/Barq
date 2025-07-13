using FluentValidation;
using BARQ.Application.Commands.Authentication;

namespace BARQ.Application.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Request.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.Request.MfaCode)
            .Length(6).WithMessage("MFA code must be 6 digits")
            .When(x => !string.IsNullOrEmpty(x.Request.MfaCode));
    }
}
