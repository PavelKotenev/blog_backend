using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blog.Domain.DTOs;

public class PostDocumentDto
{
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Column("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [Column("tags")]
    [JsonPropertyName("tags")]
    public string Tags { get; set; } = string.Empty;

    [Column("created_at")]
    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; }
}