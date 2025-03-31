using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;

public class TransferPostsToElasticsearchCommandHandler(
    IPostRepositories.IElasticCommand elasticRepository,
    IPostRepositories.IPostgresQuery postgresRepository
) : IRequestHandler<TransferPostsToElasticsearchCommand, Unit>
{
    public async Task<Unit> Handle(TransferPostsToElasticsearchCommand command, CancellationToken cancellationToken)
    {
        var postsDocuments =
            await postgresRepository.GetPostDocumentsByIds(command.Ids, cancellationToken);
        await elasticRepository.BulkCreate(postsDocuments, cancellationToken);

        return Unit.Value;
    }
}