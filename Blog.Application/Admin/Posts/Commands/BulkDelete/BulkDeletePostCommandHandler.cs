using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkDelete;

public class BulkDeletePostCommandHandler(
    IPostRepositories.IElasticCommand postElasticCommandRepository,
    IPostRepositories.IPostgresCommand postPostgresCommandRepository,
    ITagRepositories.IPostgresCommand tagPostgresCommandRepository

    ) : IRequestHandler<BulkDeletePostCommand, Unit> 
{
    public async Task<Unit> Handle(BulkDeletePostCommand command, CancellationToken cancellationToken)
    {
        await postPostgresCommandRepository.BulkDelete(command.Ids, cancellationToken);
        await postElasticCommandRepository.BulkDelete(command.Ids, cancellationToken);
        await tagPostgresCommandRepository.RefreshMvTagStatistics(cancellationToken);
        return Unit.Value;
    }
}