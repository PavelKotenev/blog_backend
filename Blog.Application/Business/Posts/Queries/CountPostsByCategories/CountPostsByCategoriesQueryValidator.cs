using System.Text.RegularExpressions;
using FluentValidation;

namespace Blog.Application.Business.Posts.Queries.CountPostsByCategories;

public partial class CountPostsByCategoriesQueryValidator : AbstractValidator<CountPostsByCategoriesQuery>
{
    public CountPostsByCategoriesQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .Must(x =>
            {
                if (string.IsNullOrEmpty(x)) return true;
                return x.Length >= 3 || ContainIdsRegex().IsMatch(x);
            })
            .WithMessage("SearchTerm must be either null, empty, contain more than 2 characters (e.g., 'abc', 'abcd'), " +
                         "or if less than 3 characters, must contain at least one digit (e.g., '13', 'a4').");

        RuleFor(x => x.FromCreatedAt)
            .GreaterThanOrEqualTo(0)
            .When(x => x.FromCreatedAt.HasValue)
            .WithMessage("FromCreatedAt must be non-negative.");

        RuleFor(x => x.ToCreatedAt)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ToCreatedAt.HasValue)
            .WithMessage("ToCreatedAt must be non-negative.")
            .GreaterThanOrEqualTo(x => x.FromCreatedAt ?? long.MinValue)
            .When(x => x.ToCreatedAt.HasValue && x.FromCreatedAt.HasValue)
            .WithMessage("ToCreatedAt must be greater than or equal to FromCreatedAt.");
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex ContainIdsRegex();
}