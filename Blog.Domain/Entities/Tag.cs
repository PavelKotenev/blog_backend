namespace Blog.Domain.Entities;

public class Tag
{
    public int Id { get; private set; }
    public required string Title { get; set; }
    public long CreatedAt { get; set; }

    public Tag(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public Tag(string title)
    {
        Title = title;
    }

    public Tag()
    {
    }
}