using Blog.Contracts.Interfaces.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Users.Commands;

public class PostgresUserCommandRepository(PostgresContext context) : IUserRepositories.IPostgresCommand
{
    public async Task<User> Create(User user)
    {
        try
        {
            context.User.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Database error: {ex.InnerException?.Message ?? ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error: {ex.Message}");
        }
    }
}