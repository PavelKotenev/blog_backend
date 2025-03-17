using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Responses;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Queries;

public class ElasticPostQueryRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticQuery
{
    private const string IndexName = "i_post";

    public async Task<SearchAllCategoriesResponse> GetSuggestions(string elasticQuery)
    {
        var response = await elasticHttpClient.PostAsync($"{IndexName}/_search", elasticQuery);
        return await ElasticResponseMapper.MapToSuggestions(response);
    }

    public async Task<GetPostsByCategoryResponse> GetPreviewPosts(string elasticQuery)
    {
        var response = await elasticHttpClient.PostAsync($"{IndexName}/_search?filter_path=hits.hits", elasticQuery);
        return await ElasticResponseMapper.MapToFilteredPosts(response);
    }
}