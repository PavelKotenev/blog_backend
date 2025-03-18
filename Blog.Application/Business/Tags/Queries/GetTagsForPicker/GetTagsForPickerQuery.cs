using Blog.Contracts.Responses;
using MediatR;

namespace Blog.Application.Business.Tags.Queries.GetTagsForPicker;

public record GetTagsForPickerQuery(
    string? SearchTerm,
    int[]? SelectedTagIds,
    int? LastTagId,
    int? LastTagPopularity
) : IRequest<GetTagsForPickerResponse>;