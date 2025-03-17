using Blog.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Business.Tags.Commands.Create;

public class CreateTagCommandHandler(
    ITagRepositories.IPostgresCommand repository
) : IRequestHandler<CreateTagCommand, Unit>
{
    public async Task<Unit> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        await repository.Create(command.Title, cancellationToken);
        return Unit.Value;
    }
}