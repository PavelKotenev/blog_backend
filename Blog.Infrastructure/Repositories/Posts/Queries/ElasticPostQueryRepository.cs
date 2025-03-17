using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Responses;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Queries;

public class ElasticPostQueryRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticQuery
{
    private const string IndexName = "i_post";


    public async Task<SearchAllCategoriesResponse> GetAllCategoriesPosts(
        string elasticQuery,
        CancellationToken cancellationToken
    )
    {
        var response = await elasticHttpClient.PostAsync(
            $"{IndexName}/_search",
            elasticQuery,
            cancellationToken
        );
        return await ElasticResponseMapper.MapToSuggestions(
            response,
            cancellationToken
        );
    }


    public async Task<GetPostsByCategoryResponse> GetCategoryPosts(
        string elasticQuery,
        CancellationToken cancellationToken
    )
    {
        var response = await elasticHttpClient.PostAsync(
            $"{IndexName}/_search?filter_path=hits.hits",
            elasticQuery,
            cancellationToken
        );
        return await ElasticResponseMapper.MapToFilteredPosts(
            response,
            cancellationToken
        );
    }
}