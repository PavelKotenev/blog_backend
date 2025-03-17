using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Responses;

namespace Blog.Domain.Interfaces.Repositories;

public interface IPostRepositories
{
    public interface IElasticCommand
    {
        public Task<HttpResponseMessage> CreatePostIndex();
        public Task<HttpResponseMessage> BulkCreate(string jsonBody);
        public Task<HttpResponseMessage> DeleteIndex();
        public Task<HttpResponseMessage> BulkDelete(string jsonBody);
    }

    public interface IElasticQuery
    {
        public Task<SearchAllCategoriesResponse> GetSuggestions(string elasticQuery);
        public Task<GetPostsByCategoryResponse> GetPreviewPosts(string elasticQuery);
    }

    public interface IPostgresCommand
    {
        public Task BulkCreate(IEnumerable<PostDto> posts);
        public Task BulkDelete(int[] ids);
        public Task Create(PostDto post);
    }

    public interface IPostgresQuery
    {
        public Task<List<PostDocumentDto>> GetPostDocumentsByIdRange(int fromId, int toId);
        public Task<List<PostDocumentDto>> GetLastPostDocument();
    }
}