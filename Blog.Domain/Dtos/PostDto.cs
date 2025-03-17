using Blog.Domain.Enums;

namespace Blog.Domain.DTOs;

public class PostDto
{
    public int? Id { get; set; }
    public int AuthorId { get; set; }
    public required ActivityStatus Status { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? Tags { get; set; }
    public long CreatedAt { get; init; }

    public PostDto()
    {
    }

    public PostDto(
        int authorId,
        ActivityStatus status,
        string title,
        string content,
        string tags
    )
    {
        AuthorId = authorId;
        Status = status;
        Title = title;
        Content = content;
        Tags = tags;
    }

    public PostDto(
        int id,
        int authorId,
        ActivityStatus status,
        string title,
        string content,
        string tags,
        long createdAt
    )
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