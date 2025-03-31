using System.Text.Json.Serialization;
using Blog.Domain.Enums;

namespace Blog.Domain.DTOs.Post;

public record CreatePostDto
{
    [JsonPropertyName("author_id")] public required int AuthorId { get; set; }
    [JsonPropertyName("status")] public required ActivityStatus Status { get; set; }
    [JsonPropertyName("title")] public required string Title { get; set; }
    [JsonPropertyName("content")] public required string Content { get; set; }
    [JsonPropertyName("tags")] public required string Tags { get; set; }
}