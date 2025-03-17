using Blog.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Mv.Commands.CreateAllMv;

public class CreateAllMvCommandHandler(ITagRepositories.IPostgresCommand repository):IRequestHandler<CreateAllMvCommand, string>
{
    public async Task<string> Handle(CreateAllMvCommand request, CancellationToken cancellationToken)
    {
        await repository.CreateMvTagStatistics();
        return "all mv created";
    }
}