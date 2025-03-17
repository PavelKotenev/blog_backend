using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Responses;

namespace Blog.Domain.Interfaces.Repositories;

public interface IPostRepositories
{
    public interface IElasticCommand
    {
        public Task<HttpResponseMessage> CreatePostIndex(CancellationToken cancellationToken);
        public Task<HttpResponseMessage> DeleteIndex(CancellationToken cancellationToken);

        public Task<HttpResponseMessage> BulkCreate(
            string jsonBody,
            CancellationToken cancellationToken
        );

        public Task<HttpResponseMessage> BulkDelete(
            string jsonBody,
            CancellationToken cancellationToken
        );
    }

    public interface IElasticQuery
    {
        public Task<SearchAllCategoriesResponse> GetAllCategoriesPosts(
            string elasticQuery,
            CancellationToken cancellationToken
        );


        public Task<GetPostsByCategoryResponse> GetCategoryPosts(
            string elasticQuery,
            CancellationToken cancellationToken
        );
    }

    public interface IPostgresCommand
    {
        public Task BulkCreate(
            IEnumerable<PostDto> posts,
            CancellationToken cancellationToken
        );


        public Task BulkDelete(
            int[] ids,
            CancellationToken cancellationToken
        );


        public Task Create(
            PostDto post,
            CancellationToken cancellationToken
        );
    }

    public interface IPostgresQuery
    {
        public Task<List<PostDocumentDto>> GetPostDocumentsByIdRange(
            int fromId,
            int toId,
            CancellationToken cancellationToken
        );

        public Task<List<PostDocumentDto>> GetLastPostDocument(CancellationToken cancellationToken);
    }
}