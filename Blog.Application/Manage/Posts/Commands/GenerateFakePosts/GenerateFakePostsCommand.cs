using System.Text.Json.Serialization;
using MediatR;

namespace Blog.Application.Manage.Posts.Commands.GenerateFakePosts;

public record GenerateFakePostsCommand(
    [property: JsonPropertyName("quantity")]
    int Quantity
) : IRequest<Unit>;