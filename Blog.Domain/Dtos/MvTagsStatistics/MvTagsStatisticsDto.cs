using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blog.Domain.DTOs.MvTagsStatistics;

public record MvTagsStatisticsDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; } = null!;

    [JsonPropertyName("postsQuantity")]
    [Column("posts_quantity")]
    public int PostsQuantity { get; set; }

    [JsonPropertyName("popularity")] public int Popularity { get; set; }
}