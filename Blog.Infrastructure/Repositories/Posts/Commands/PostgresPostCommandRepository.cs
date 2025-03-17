using System.Text;
using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class PostgresPostCommandRepository(PostgresContext context) : IPostRepositories.IPostgresCommand
{
    public async Task BulkCreate(
        IEnumerable<PostDto> posts,
        CancellationToken cancellationToken = default
    )
    {
        var postDtos = posts.ToList();
        if (postDtos.Count == 0)
            return;

        var sql = new StringBuilder(
            "INSERT INTO t_posts (author_id, title, content, status, tags, created_at) VALUES ");
        var values = (from post in postDtos
            let authorId = post.AuthorId
            let title = post.Title.Replace("'", "''")
            let content = post.Content
            let status = post.Status
            let tags = post.Tags
            let createdAt = post.CreatedAt
            select $"('{authorId}', '{title}', {content}, {status}, '{tags}', {createdAt})").ToList();

        sql.Append(string.Join(", ", values));
        sql.Append(';');

        await context.Database.ExecuteSqlRawAsync(sql.ToString(), cancellationToken);
    }

    public async Task BulkDelete(
        int[] ids,
        CancellationToken cancellationToken
    )
    {
        if (ids.Length == 0)
            return;

        await context.Post
            .Where(p => ids.Contains(p.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task Create(
        PostDto post,
        CancellationToken cancellationToken
    )
    {
        if (post == null)
            throw new ArgumentNullException(nameof(post));

        var postEntity = new Post(
            post.AuthorId,
            post.Status,
            post.Title,
            post.Content,
            post.Tags
        );

        context.Post.Add(postEntity);
        await context.SaveChangesAsync(cancellationToken);
    }
}