using System.Text;
using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class PostgresPostCommandRepository(PostgresContext context) : IPostRepositories.IPostgresCommand
{
    public async Task BulkCreate(IEnumerable<PostDto> posts)
    {
        var sql = new StringBuilder("INSERT INTO t_posts (author_id, title, content, status, tags, created_at) VALUES ");
        var values = new List<string>();

        foreach (var post in posts)
        {
            var authorId = post.AuthorId;
            var title = post.Title.Replace("'", "''");
            var content = post.Content?.Replace("'", "''");
            var status = post.Status;
            var tags = post.Tags;
            var createdAt = post.CreatedAt;

            values.Add($"('{authorId}', '{title}', '{content}', {status},  '{tags}', '{createdAt}')");
        }

        sql.Append(string.Join(", ", values));
        sql.Append(';');

        await context.Database.ExecuteSqlRawAsync(sql.ToString());
    }

    public async Task BulkDelete(int[] ids)
    {
        await context.Post
            .Where(p => ids.Contains(p.Id))
            .ExecuteDeleteAsync();
    }

    public async Task Create(PostDto post)
    {
        var postEntity = new Post
        (
            post.AuthorId,
            post.Status,
            post.Title,
            post.Content,
            post.Tags
        );

        context.Post.Add(postEntity);
        await context.SaveChangesAsync();
    }
}