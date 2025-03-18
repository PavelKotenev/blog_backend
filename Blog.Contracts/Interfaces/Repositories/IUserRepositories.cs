using Blog.Domain.Entities;

namespace Blog.Contracts.Interfaces.Repositories;
public interface IUserRepositories
{
    public interface IPostgresCommand
    {
        public Task<User> Create(User user);
    }
}