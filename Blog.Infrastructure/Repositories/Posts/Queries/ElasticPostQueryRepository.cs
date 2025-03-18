using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Queries;

public class ElasticPostQueryRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticQuery
{
    private const string IndexName = "i_post";


    public async Task<GetAllCategoriesPostsResponse> GetAllCategoriesPosts(
        string elasticQuery,
        CancellationToken cancellationToken
    )
    {
        var response = await elasticHttpClient.PostAsync(
            $"{IndexName}/_search",
            elasticQuery,
            cancellationToken
        );
        return await ElasticResponseMapper.MapToAllCatgoriesPostsResponse(
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
        return await ElasticResponseMapper.MapToPostsByCategoryResponse(
            response,
            cancellationToken
        );
    }
}