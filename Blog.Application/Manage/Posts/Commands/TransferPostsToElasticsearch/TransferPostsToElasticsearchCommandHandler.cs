using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;

public class TransferPostsToElasticsearchCommandHandler(
    IPostRepositories.IElasticCommand elasticRepository,
    IPostRepositories.IPostgresQuery postgresRepository
) : IRequestHandler<TransferPostsToElasticsearchCommand, Unit>
{
    public async Task<Unit> Handle(TransferPostsToElasticsearchCommand command, CancellationToken cancellationToken)
    {
        var postsDocumentsDtos = await postgresRepository.GetPostDocumentsByIdRange(command.FromId, command.ToId);
        var jsonBody = ElasticQueryBuilder.BuildBulkCreateQuery(postsDocumentsDtos);
        await elasticRepository.BulkCreate(jsonBody);

        return Unit.Value;
    }
}