using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class ElasticPostCommandRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticCommand
{
    private const string IndexName = "i_post";

    public async Task<HttpResponseMessage> BulkDelete(string jsonBody)
    {
        return await elasticHttpClient.PostAsync($"{IndexName}/_delete_by_query", jsonBody);
    }

    public async Task<HttpResponseMessage> BulkCreate(string jsonBody)
    {
        return await elasticHttpClient.PostAsync("_bulk", jsonBody);
    }

    public async Task<HttpResponseMessage> CreatePostIndex()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../Blog.Infrastructure/i_post.json");
        var jsonBody = await File.ReadAllTextAsync(filePath);

        return await elasticHttpClient.PutAsync(IndexName, jsonBody);
    }
    
    public async Task<HttpResponseMessage> DeleteIndex()
    {
        return await elasticHttpClient.DeleteAsync(IndexName);
    }
}