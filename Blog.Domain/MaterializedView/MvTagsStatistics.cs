using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blog.Domain.MaterializedView;

public class MvTagsStatistics
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; } = null!;

    [JsonPropertyName("postsQuantity")]
    [Column("posts_quantity")]
    public int PostsQuantity { get; set; }

    [JsonPropertyName("popularity")] public int Popularity { get; set; }

    [JsonPropertyName("createdAt")]
    [Column("created_at")]
    public long CreatedAt { get; set; }

    public MvTagsStatistics(int id, string title, int postsQuantity, int popularity, long createdAt)
    {
        Id = id;
        Title = title;
        PostsQuantity = postsQuantity;
        Popularity = popularity;
        CreatedAt = createdAt;
    }

    public MvTagsStatistics()
    {
    }
}