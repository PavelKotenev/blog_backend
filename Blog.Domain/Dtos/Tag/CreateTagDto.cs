using System.Text.Json.Serialization;

namespace Blog.Domain.Dtos.Tag;

public record CreateTagDto
{
    [JsonPropertyName("title")] public required string Title { get; set; }

}