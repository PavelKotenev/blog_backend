using Blog.Contracts.Interfaces.Repositories;
using Blog.Domain.DTOs;
using Blog.Domain.DTOs.Post;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class ElasticPostCommandRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticCommand
{
    private const string IndexName = "i_post";

    public async Task<HttpResponseMessage> BulkDelete(int[] ids, CancellationToken cancellationToken)
    {
        var jsonBody = ElasticQueryBuilder.BuildBulkDeleteQuery(ids);
        Console.WriteLine(jsonBody);
        return await elasticHttpClient.PostAsync($"{IndexName}/_delete_by_query", jsonBody, cancellationToken);
    }

    public async Task<HttpResponseMessage> BulkUpdate(List<UpdatePostDto> post, CancellationToken cancellationToken)
    {
        var jsonBody = ElasticQueryBuilder.BuildBulkUpdateQuery(post);
        return await elasticHttpClient.PostAsync("_bulk", jsonBody, cancellationToken);
    }

    public async Task<HttpResponseMessage> BulkCreate(List<PostDocumentDto> postsDocuments,
        CancellationToken cancellationToken)
    {
        var jsonBody = ElasticQueryBuilder.BuildBulkCreateQuery(postsDocuments);
        return await elasticHttpClient.PostAsync("_bulk", jsonBody, cancellationToken);
    }


    public async Task<HttpResponseMessage> CreateIndex(CancellationToken cancellationToken)
    {
        var jsonBody = ElasticQueryBuilder.BuildCreatePostIndexQuery();
        return await elasticHttpClient.PutAsync(IndexName, jsonBody, cancellationToken);
    }


    public async Task<HttpResponseMessage> DeleteIndex(CancellationToken cancellationToken)
    {
        return await elasticHttpClient.DeleteAsync(IndexName, cancellationToken);
    }
}