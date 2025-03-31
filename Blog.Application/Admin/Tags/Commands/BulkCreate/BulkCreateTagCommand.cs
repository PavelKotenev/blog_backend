using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Admin.Tags.Commands.BulkCreate;

public record BulkCreateTagCommand(
    [property: JsonPropertyName("title")] string Title
) : IRequest<Unit>;