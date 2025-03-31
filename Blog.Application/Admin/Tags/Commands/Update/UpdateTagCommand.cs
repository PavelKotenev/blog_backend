using MediatR;

namespace Blog.Application.Admin.Tags.Commands.Update;

public record UpdateTagCommand(
    
    ) : IRequest<Unit>;