using FluentValidation;

namespace Blog.Application.Business.Tags.Queries.GetTagsForPicker;

public class GetTagsForPickerQueryValidator : AbstractValidator<GetTagsForPickerQuery>
{
    public GetTagsForPickerQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .Must(searchTerm =>
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return true;
                }

                var trimmedLength = searchTerm.Replace(" ", "").Length;
                return trimmedLength >= 2;
            })
            .When(x => x.SearchTerm != null)
            .WithMessage("SearchTerm must contain at least 2 non-space characters if provided.");
    }
}