using Blog.Application.Business.Posts.Queries.GetAllCategoriesPosts;
using FluentValidation.TestHelper;

namespace Blog.Tests;

public class GetAllCategoriesPostsQueryValidatorTests
{
    private readonly GetAllCategoriesPostsQueryValidator _validator = new();

    [Fact]
    public void Should_Fail_Validation_When_SearchTerm_Is_OneLetter()
    {
        var query = new GetAllCategoriesPostsQuery("a", null, null, null);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SearchTerm)
            .WithErrorMessage("SearchTerm must meet one of the following rules: " +
                              "1) 3 or more characters with the first 3 being letters (e.g., 'abc', 'abcd'); " +
                              "2) Exactly 3 characters with 2 letters and a space (e.g., 'ab ', 'a b', ' ab'); " +
                              "3) Shorter than 3 characters but starts with '#' (e.g., '#a', '#') or contains at least one digit (e.g., 'a1', '1').");
    }
}