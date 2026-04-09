using FluentValidation;
using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application.Validators.Users;

public sealed class UsersListQueryValidator : AbstractValidator<UsersListQuery>
{
    private static readonly string[] AllowedSortBy = ["name", "email", "role", "id"];

    public UsersListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortOrder)
            .Must(value => string.IsNullOrWhiteSpace(value) ||
                           value.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                           value.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortOrder must be 'asc' or 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(value => string.IsNullOrWhiteSpace(value) ||
                           AllowedSortBy.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: name, email, role, id.");
    }
}
