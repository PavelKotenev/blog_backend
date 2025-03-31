using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkUpdate;

public class BulkUpdatePostCommandHandler(
    IPostRepositories.IElasticCommand postElasticCommandRepository,
    IPostRepositories.IPostgresCommand postPostgresCommandRepository,
    ITagRepositories.IPostgresCommand tagPostgresCommandRepository
) : IRequestHandler<BulkUpdatePostCommand, Unit>
{
    public async Task<Unit> Handle(BulkUpdatePostCommand command, CancellationToken cancellationToken)
    {
        await postPostgresCommandRepository.BulkUpdate(command.Posts, cancellationToken);
        await postElasticCommandRepository.BulkUpdate(command.Posts, cancellationToken);
        await tagPostgresCommandRepository.RefreshMvTagStatistics(cancellationToken);
        return Unit.Value;
    }
}