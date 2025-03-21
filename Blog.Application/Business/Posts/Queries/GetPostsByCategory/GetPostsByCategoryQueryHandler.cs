using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetPostsByCategory;

public class GetPostsByCategoryQueryHandler(
    IPostRepositories.IElasticQuery repository
) : IRequestHandler<GetPostsByCategoryQuery, GetPostsByCategoryResponse>
{
    public async Task<GetPostsByCategoryResponse> Handle(
        GetPostsByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        var elasticQuery = ElasticQueryBuilder
            .BuildPostsByCategoryQuery(
                query.Category,
                query.SearchTerm,
                query.FromCreatedAt,
                query.ToCreatedAt,
                query.LastPostId,
                query.LastPostCreatedAt,
                query.SelectedTags
            );
        return await repository.GetPostsByCategory(
            elasticQuery,
            cancellationToken
        );
    }
}