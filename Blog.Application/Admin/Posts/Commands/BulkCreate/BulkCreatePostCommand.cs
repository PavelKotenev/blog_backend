using System.Text.Json.Serialization;
using Blog.Domain.DTOs;
using Blog.Domain.DTOs.Post;
using MediatR;

namespace Blog.Application.Admin.Posts.Commands.BulkCreate;

public record BulkCreatePostCommand(
    [property: JsonPropertyName("posts")] List<CreatePostDto> Posts
) : IRequest<Unit>;