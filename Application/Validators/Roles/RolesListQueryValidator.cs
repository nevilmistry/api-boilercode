using FluentValidation;
using GenricRepository.Application.Contracts.Roles;

namespace GenricRepository.Application.Validators.Roles;

public sealed class RolesListQueryValidator : AbstractValidator<RolesListQuery>
{
    private static readonly string[] AllowedSortBy = ["name", "id"];

    public RolesListQueryValidator()
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
            .WithMessage("SortBy must be one of: name, id.");
    }
}
