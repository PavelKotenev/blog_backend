using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkCreate;

public class BulkCreatePostCommandHandler(
    IPostRepositories.IElasticCommand postElasticRepository,
    IPostRepositories.IPostgresCommand postPostgresCommandRepository,
    IPostRepositories.IPostgresQuery postPostgresQueryRepository,
    ITagRepositories.IPostgresCommand tagPostgresCommandRepository
) : IRequestHandler<BulkCreatePostCommand, Unit>
{
    public async Task<Unit> Handle(BulkCreatePostCommand command, CancellationToken cancellationToken)
    {
        var ids = await postPostgresCommandRepository.BulkCreate(command.Posts, cancellationToken);

        var postsDocuments = await postPostgresQueryRepository.GetPostDocumentsByIds(
            ids,
            cancellationToken
        );

        await postElasticRepository.BulkCreate(postsDocuments, cancellationToken);
        await tagPostgresCommandRepository.RefreshMvTagStatistics(cancellationToken);

        return Unit.Value;
    }
}