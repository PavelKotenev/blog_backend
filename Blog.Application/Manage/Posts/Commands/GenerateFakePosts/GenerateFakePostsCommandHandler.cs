using Blog.Contracts.Interfaces.Repositories;
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
            var createPostDtos = FakerService.CreateFakePosts(command.Quantity);
            await repository.BulkCreate(createPostDtos, cancellationToken);
            return Unit.Value;
        }
    }
}