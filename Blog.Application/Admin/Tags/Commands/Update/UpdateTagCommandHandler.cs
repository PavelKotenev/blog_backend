using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Admin.Tags.Commands.Update;

public class UpdateTagCommandHandler(
    ITagRepositories.IPostgresCommand repository
) : IRequestHandler<UpdateTagCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}