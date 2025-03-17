using MediatR;

namespace Blog.Application.Manage.Posts.Commands.DeletePostIndex;

public record DeletePostIndexCommand : IRequest<Unit>;