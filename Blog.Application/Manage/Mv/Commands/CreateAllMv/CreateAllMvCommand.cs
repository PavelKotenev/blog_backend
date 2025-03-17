using MediatR;

namespace Blog.Application.Manage.Mv.Commands.CreateAllMv;

public record CreateAllMvCommand() : IRequest<string>;