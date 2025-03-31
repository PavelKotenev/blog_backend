using Blog.Domain.DTOs.Post;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkUpdate;

public record BulkUpdatePostCommand(
    List<UpdatePostDto> Posts
) : IRequest<Unit>;