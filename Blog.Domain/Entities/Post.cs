using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; private set; }
    public int AuthorId { get; init; }
    public ActivityStatus Status { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public string? Tags { get; init; }
    public long CreatedAt { get; init; }

    public Post(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public Post(int authorId, ActivityStatus status, string title, string content, string? tags)
    {
        AuthorId = authorId;
        Status = status;
        Title = title;
        Content = content;
        Tags = tags;
    }

    public Post(int id, int authorId, ActivityStatus status, string title, string content, string? tags,
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