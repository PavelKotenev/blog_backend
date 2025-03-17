using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Services;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.GenerateFakePosts
{
    public class GenerateFakePostsCommandHandler(
        IPostRepositories.IPostgresCommand repository
    ) : IRequestHandler<GenerateFakePostsCommand, Unit>
    {
        public async Task<Unit> Handle(GenerateFakePostsCommand command, CancellationToken cancellationToken)
        {
            var postsEntities = FakerService.CreateFakePosts(command.Quantity);
            await repository.BulkCreate(postsEntities, cancellationToken);
            return Unit.Value;
        }
    }
}