using Blog.Contracts.Interfaces.Repositories;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.CreatePostIndex
{
    public class CreatePostIndexCommandHandler(
        IPostRepositories.IElasticCommand repository
    ) : IRequestHandler<CreatePostIndexCommand, Unit>
    {
        public async Task<Unit> Handle(CreatePostIndexCommand command, CancellationToken cancellationToken)
        {
            await repository.CreatePostIndex(cancellationToken);
            return Unit.Value;
        }
    }
}