using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.DeletePostIndex;

public class DeletePostIndexCommandHandler(IPostRepositories.IElasticCommand repository)
    : IRequestHandler<DeletePostIndexCommand, Unit>
{
    public async Task<Unit> Handle(DeletePostIndexCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteIndex(cancellationToken);
        return Unit.Value;
    }
}