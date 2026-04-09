using FluentValidation;
using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application.Validators.Users;

public sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name is required.")
            .MaximumLength(120);

        RuleFor(x => x.Email)
            .Must(email => !string.IsNullOrWhiteSpace(email))
            .WithMessage("Email is required.")
            .EmailAddress()
            .MaximumLength(160);

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required.");
    }
}
