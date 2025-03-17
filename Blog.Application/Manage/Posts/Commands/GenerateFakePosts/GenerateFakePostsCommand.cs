using MediatR;

namespace Blog.Application.Manage.Posts.Commands.GenerateFakePosts;

public record GenerateFakePostsCommand(int Quantity) : IRequest<Unit>;