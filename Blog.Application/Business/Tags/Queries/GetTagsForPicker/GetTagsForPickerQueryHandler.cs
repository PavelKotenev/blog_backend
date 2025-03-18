using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using MediatR;

namespace Blog.Application.Business.Tags.Queries.GetTagsForPicker;

public class GetTagsForAdminTableQueryHandler(
    ITagRepositories.IPostgresQuery repository
) : IRequestHandler<GetTagsForPickerQuery, GetTagsForPickerResponse>
{
    public async Task<GetTagsForPickerResponse> Handle(GetTagsForPickerQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetTagsPickerTags(
            query.LastTagId,
            query.LastTagPopularity,
            query.SelectedTagIds,
            cancellationToken
        );
    }
}