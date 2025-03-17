using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;

public interface IUserRepositories
{
    public interface IPostgresCommand
    {
        public Task<User> Create(User user);
    }
}