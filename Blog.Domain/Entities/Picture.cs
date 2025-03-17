namespace Blog.Domain.Entities;

public class Picture
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public required string ImageUrl { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required string ImageType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Post Post { get; set; }
}