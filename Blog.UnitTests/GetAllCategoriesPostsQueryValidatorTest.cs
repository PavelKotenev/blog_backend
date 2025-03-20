using Blog.Application.Business.Posts.Queries.CountPostsByCategories;
using FluentValidation.TestHelper;

namespace Blog.Tests;

public class CountPostsByCategoriesQueryValidatorTests
{
    private readonly CountPostsByCategoriesQueryValidator _validator = new();

    [Fact]
    public void Should_Fail_Validation_When_SearchTerm_Is_Less_Than_Three_Characters_Without_Digits()
    {
        var query = new CountPostsByCategoriesQuery("ab", null, null, null);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SearchTerm)
            .WithErrorMessage(
                "SearchTerm must be either null, empty, contain more than 2 characters (e.g., 'abc', 'abcd'), " +
                "or if less than 3 characters, must contain at least one digit (e.g., '13', 'a4').");
    }
}