using MediatR;

namespace Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;

public record TransferPostsToElasticsearchCommand(int FromId, int ToId) : IRequest<Unit>;