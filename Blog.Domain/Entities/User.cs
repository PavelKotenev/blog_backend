namespace Blog.Domain.Entities;

public class User
{
    public Guid Guid { get; init; }
    public required string UserName { get; init; }
    public string? NormalizedUserName { get; init; }
    public required string Email { get; init; }
    public string? NormalizedEmail { get; init; }
    public bool EmailConfirmed { get; init; }
    public required string PasswordHash { get; init; }
    public string? SecurityStamp { get; init; }
    public string? ConcurrencyStamp { get; init; }
    public string? PhoneNumber { get; init; }
    public bool PhoneNumberConfirmed { get; init; }
    public bool TwoFactorEnabled { get; init; }
    public DateTimeOffset? LockoutEnd { get; init; }
    public bool LockoutEnabled { get; init; }
    public int AccessFailedCount { get; init; }
}