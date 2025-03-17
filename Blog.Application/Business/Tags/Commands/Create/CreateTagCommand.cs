using MediatR;

namespace Blog.Application.Business.Tags.Commands.Create;

public record CreateTagCommand(string Title) : IRequest<Unit>;