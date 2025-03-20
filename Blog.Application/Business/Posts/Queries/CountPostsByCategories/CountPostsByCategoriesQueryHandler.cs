using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.CountPostsByCategories;

public class CountPostsByCategoriesQueryHandler(
    IPostRepositories.IElasticQuery queryRepository
) : IRequestHandler<CountPostsByCategoriesQuery, CountPostsByCategoriesResponse>
{
    public async Task<CountPostsByCategoriesResponse> Handle(
        CountPostsByCategoriesQuery byCategoriesQuery,
        CancellationToken cancellationToken
        )
    {
        var elasticQuery = ElasticQueryBuilder.BuildCountPostsByCategoryQuery(
            byCategoriesQuery.SearchTerm,
            byCategoriesQuery.FromCreatedAt,
            byCategoriesQuery.ToCreatedAt,
            byCategoriesQuery.SelectedTags
            );
        Console.WriteLine(elasticQuery);
        return await queryRepository.CountPostsByCaterogies(elasticQuery, cancellationToken);
    }
}