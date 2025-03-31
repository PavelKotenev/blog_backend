using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Tags.Commands.BulkDelete;

public class BulkDeleteTagCommandHandler(
    ITagRepositories.IPostgresCommand repository
) : IRequestHandler<BulkDeleteTagCommand, Unit>
{
    public async Task<Unit> Handle(BulkDeleteTagCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}