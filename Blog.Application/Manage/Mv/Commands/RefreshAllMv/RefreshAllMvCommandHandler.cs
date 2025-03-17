using Blog.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Mv.Commands.RefreshAllMv;

public class RefreshAllMvCommandHandler(ITagRepositories.IPostgresCommand repository):IRequestHandler<RefreshAllMvCommand, string>
{
    public async Task<string> Handle(RefreshAllMvCommand request, CancellationToken cancellationToken)
    {
        await repository.RefreshMvTagStatistics();
        return "all mv refreshed";
    }
}