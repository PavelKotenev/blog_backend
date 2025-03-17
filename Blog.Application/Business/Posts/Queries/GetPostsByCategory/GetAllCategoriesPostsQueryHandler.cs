using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Responses;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetPostsByCategory;

public class GetAllCategoriesPostsQueryHandler(
    IPostRepositories.IElasticQuery repository
) : IRequestHandler<GetPostsByCategoryQuery, GetPostsByCategoryResponse>
{
    public async Task<GetPostsByCategoryResponse> Handle(GetPostsByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        var elasticQuery = ElasticQueryBuilder
            .BuildFilteredPostsQuery(
                query.Category,
                query.SearchTerm,
                query.FromCreatedAt,
                query.ToCreatedAt,
                query.LastPostId,
                query.LastPostCreatedAt,
                query.SelectedTags
            );
        return await repository.GetPreviewPosts(elasticQuery);
    }
}