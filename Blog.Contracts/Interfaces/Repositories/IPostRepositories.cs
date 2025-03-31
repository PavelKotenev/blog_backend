using Blog.Contracts.Responses;
using Blog.Domain.DTOs;
using Blog.Domain.DTOs.Post;

namespace Blog.Contracts.Interfaces.Repositories;

public interface IPostRepositories
{
    public interface IElasticCommand
    {
        public Task<HttpResponseMessage> CreateIndex(CancellationToken cancellationToken);
        public Task<HttpResponseMessage> DeleteIndex(CancellationToken cancellationToken);

        public Task<HttpResponseMessage> BulkUpdate(
            List<UpdatePostDto> post,
            CancellationToken cancellationToken
        );

        public Task<HttpResponseMessage> BulkCreate(
            List<PostDocumentDto> postsDocuments,
            CancellationToken cancellationToken
        );

        public Task<HttpResponseMessage> BulkDelete(
            int[] ids,
            CancellationToken cancellationToken
        );
    }

    public interface IElasticQuery
    {
        public Task<CountPostsByCategoriesResponse> CountPostsByCategories(
            string elasticQuery,
            CancellationToken cancellationToken
        );


        public Task<GetPostsByCategoryResponse> GetPostsByCategory(
            string elasticQuery,
            CancellationToken cancellationToken
        );
    }

    public interface IPostgresCommand
    {
        public Task<int[]> BulkCreate(
            List<CreatePostDto> posts,
            CancellationToken cancellationToken
        );

        public Task BulkDelete(
            int[] ids,
            CancellationToken cancellationToken
        );

        public Task BulkUpdate(
            List<UpdatePostDto> post,
            CancellationToken cancellationToken
        );
    }

    public interface IPostgresQuery
    {
        public Task<List<PostDocumentDto>> GetPostDocumentsByIds(
            int[] ids,
            CancellationToken cancellationToken
        );

        public Task<List<PostDocumentDto>> GetLastPostDocument(CancellationToken cancellationToken);
    }
}