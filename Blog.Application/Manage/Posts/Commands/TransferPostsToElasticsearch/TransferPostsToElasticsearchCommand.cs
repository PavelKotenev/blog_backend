using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;

public record TransferPostsToElasticsearchCommand(
    [property: JsonPropertyName("ids")]
    int[] Ids
) : IRequest<Unit>;