using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Admin.Tags.Commands.BulkDelete;

public record BulkDeleteTagCommand(
    [property: JsonPropertyName("ids")] int[] Ids
) : IRequest<Unit>;