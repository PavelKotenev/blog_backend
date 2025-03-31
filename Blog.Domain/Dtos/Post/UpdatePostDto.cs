using System.Text.Json.Serialization;
using Blog.Domain.Enums;

namespace Blog.Domain.DTOs.Post;

public record UpdatePostDto
{
    [JsonPropertyName("id")] public required int Id { get; set; }
    [JsonPropertyName("author_id")] public int? AuthorId { get; set; }
    [JsonPropertyName("status")] public ActivityStatus? Status { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("tags")] public string? Tags { get; set; }
};