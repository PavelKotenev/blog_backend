namespace Blog.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Handle { get; set; }
    public int? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
