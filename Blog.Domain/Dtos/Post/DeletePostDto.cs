using System.Text.Json.Serialization;
using Blog.Domain.Enums;

namespace Blog.Domain.DTOs.Post;

public record DeletePostDto
{
    [JsonPropertyName("id")] public required int Id { get; set; }
};