using MediatR;

namespace Blog.Application.Manage.Posts.Commands.CreatePostIndex;

public record CreatePostIndexCommand : IRequest<Unit>;