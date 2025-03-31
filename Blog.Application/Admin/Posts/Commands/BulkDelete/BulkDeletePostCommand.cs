using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkDelete;

public record BulkDeletePostCommand(
    [property: JsonPropertyName("ids")] int[] Ids
) : IRequest<Unit>;