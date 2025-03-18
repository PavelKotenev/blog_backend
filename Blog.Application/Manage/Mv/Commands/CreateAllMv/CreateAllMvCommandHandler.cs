using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Mv.Commands.CreateAllMv;

public class CreateAllMvCommandHandler(ITagRepositories.IPostgresCommand repository):IRequestHandler<CreateAllMvCommand, Unit>
{
    public async Task<Unit> Handle(CreateAllMvCommand request, CancellationToken cancellationToken)
    {
        await repository.CreateMvTagStatistics(cancellationToken);
        return Unit.Value;
    }
}