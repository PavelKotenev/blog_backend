﻿using Blog.Contracts.Interfaces.Repositories;
using Blog.Infrastructure.Services;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class ElasticPostCommandRepository(ElasticHttpClient elasticHttpClient) : IPostRepositories.IElasticCommand
{
    private const string IndexName = "i_post";


    public async Task<HttpResponseMessage> BulkDelete(string jsonBody, CancellationToken cancellationToken) {
        return await elasticHttpClient.PostAsync($"{IndexName}/_delete_by_query", jsonBody, cancellationToken);
    }


    public async Task<HttpResponseMessage> BulkCreate(string jsonBody, CancellationToken cancellationToken) {
        return await elasticHttpClient.PostAsync("_bulk", jsonBody, cancellationToken);
    }


    public async Task<HttpResponseMessage> CreatePostIndex(CancellationToken cancellationToken) {
        var jsonBody = ElasticQueryBuilder.BuildCreatePostIndexQuery();
        return await elasticHttpClient.PutAsync(IndexName, jsonBody, cancellationToken);
    }


    public async Task<HttpResponseMessage> DeleteIndex(CancellationToken cancellationToken) {
        return await elasticHttpClient.DeleteAsync(IndexName, cancellationToken);
    }
}