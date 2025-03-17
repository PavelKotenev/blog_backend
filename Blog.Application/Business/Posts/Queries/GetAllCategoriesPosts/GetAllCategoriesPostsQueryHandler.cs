using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Responses;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetAllCategoriesPosts;

public class GetAllCategoriesPostsQueryHandler(
    IPostRepositories.IElasticQuery queryRepository
) : IRequestHandler<GetAllCategoriesPostsQuery, SearchAllCategoriesResponse>
{
    public async Task<SearchAllCategoriesResponse> Handle(
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