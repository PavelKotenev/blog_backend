using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Tags.Commands.BulkCreate;

public class BulkCreateTagCommandHandler(
    ITagRepositories.IPostgresCommand repository
) : IRequestHandler<BulkCreateTagCommand, Unit>
{
    public async Task<Unit> Handle(BulkCreateTagCommand command, CancellationToken cancellationToken)
    {
        await repository.Create(command.Title, cancellationToken);
        await repository.RefreshMvTagStatistics(cancellationToken);
        return Unit.Value;
    }
}