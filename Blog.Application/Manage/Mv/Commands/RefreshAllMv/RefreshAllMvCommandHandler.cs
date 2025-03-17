using Blog.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Mv.Commands.RefreshAllMv;

public class RefreshAllMvCommandHandler(ITagRepositories.IPostgresCommand repository):IRequestHandler<RefreshAllMvCommand, Unit>
{
    public async Task<Unit> Handle(RefreshAllMvCommand request, CancellationToken cancellationToken)
    {
        await repository.RefreshMvTagStatistics(cancellationToken);
        return Unit.Value;
    }
}