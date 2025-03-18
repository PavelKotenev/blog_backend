using System.Text.RegularExpressions;
using FluentValidation;

namespace Blog.Application.Business.Posts.Queries.GetPostsByCategory;

public partial class GetPostsByCategoryQueryValidator: AbstractValidator<GetPostsByCategoryQuery>
{
    public GetPostsByCategoryQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotNull()
            .WithMessage("SearchTerm cannot be null.")
            .Must(IsValidSearchTerm)
            .WithMessage("SearchTerm must meet one of the following rules: " +
                         "1) 3 or more characters with the first 3 being letters (e.g., 'abc', 'abcd'); " +
                         "2) Exactly 3 characters with 2 letters and a space (e.g., 'ab ', 'a b', ' ab'); " +
                         "3) Shorter than 3 characters but starts with '#' (e.g., '#a', '#') or contains at least one digit (e.g., 'a1', '1').");

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

    /// <summary>
    /// Checks if the string meets the specified validation rules.
    /// </summary>
    /// <param name="queryString">The input string to validate.</param>
    /// <returns>true if the string is valid, otherwise false.</returns>
    /// <remarks>
    /// Rules:
    /// 1. The string has 3 or more characters, and the first 3 are letters (e.g., "abc", "abcd").
    /// 2. The string has exactly 3 characters: 2 letters and a space (e.g., "ab ", "a b", " ab").
    /// 3. The string is shorter than 3 characters but starts with "#" (e.g., "#a", "#") or contains at least one digit (e.g., "a1", "1").
    /// </remarks>
    private static bool IsValidSearchTerm(string? queryString)
    {
        // Если queryString null, метод вернет false, но .When уже фильтрует это
        return queryString != null && IsValidSearchTermRegex().IsMatch(queryString);
    }

    [GeneratedRegex(@"^([a-zA-Z]{3,}|([a-zA-Z]{2}\s[a-zA-Z]{1}|\s[a-zA-Z]{2}[a-zA-Z]{1})|#|\d.*)$")]
    private static partial Regex IsValidSearchTermRegex();
}