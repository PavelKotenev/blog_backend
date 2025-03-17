using MediatR;

namespace Blog.Application.Manage.Mv.Commands.RefreshAllMv;

public record RefreshAllMvCommand() : IRequest<Unit>;