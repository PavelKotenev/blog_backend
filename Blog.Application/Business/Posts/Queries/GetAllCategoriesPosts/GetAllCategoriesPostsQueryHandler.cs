using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetAllCategoriesPosts;

public class GetAllCategoriesPostsQueryHandler(
    IPostRepositories.IElasticQuery queryRepository
) : IRequestHandler<GetAllCategoriesPostsQuery, GetAllCategoriesPostsResponse>
{
    public async Task<GetAllCategoriesPostsResponse> Handle(
        GetAllCategoriesPostsQuery query,
        CancellationToken cancellationToken
        )
    {
        var elasticQuery = ElasticQueryBuilder.BuildPostsAggregationsQuery(
            query.SearchTerm,
            query.FromCreatedAt,
            query.ToCreatedAt,
            query.SelectedTags
            );
        return await queryRepository.GetAllCategoriesPosts(elasticQuery, cancellationToken);
    }
}