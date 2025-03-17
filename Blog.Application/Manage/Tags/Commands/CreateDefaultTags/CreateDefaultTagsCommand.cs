using MediatR;

namespace Blog.Application.Manage.Tags.Commands.CreateDefaultTags;

public record CreateDefaultTagsCommand : IRequest<Unit>;