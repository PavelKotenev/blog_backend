using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Tags.Commands.CreateDefaultTags;

public class CreateDefaultTagsCommandHandler(ITagRepositories.IPostgresCommand repository)
    : IRequestHandler<CreateDefaultTagsCommand, Unit>
{
    public async Task<Unit> Handle(CreateDefaultTagsCommand command, CancellationToken cancellationToken)
    {
        await repository.CreateDefaultTags(cancellationToken);
        return Unit.Value;
    }
}