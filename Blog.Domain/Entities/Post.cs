namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; private set; }
    public int AuthorId { get; set; }
    public int Status { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? Tags { get; set; }
    public long CreatedAt { get; set; }

    public Post(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public Post(int authorId, int status, string title, string content, string? tags)
    {
        AuthorId = authorId;
        Status = status;
        Title = title;
        Content = content;
        Tags = tags;
    }

    public Post(int id, int authorId, int status, string title, string content, string? tags,
        long createdAt)
    {
        Id = id;
        AuthorId = authorId;
        Status = status;
        Title = title;
        Content = content;
        Tags = tags;
        CreatedAt = createdAt;
    }
}