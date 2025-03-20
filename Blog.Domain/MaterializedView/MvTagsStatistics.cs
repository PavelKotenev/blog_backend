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

    public MvTagsStatistics(int id, string title, int postsQuantity, int popularity)
    {
        Id = id;
        Title = title;
        PostsQuantity = postsQuantity;
        Popularity = popularity;
    }

    public MvTagsStatistics()
    {
    }
}