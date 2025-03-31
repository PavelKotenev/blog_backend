using System.Text;
using Blog.Contracts.Interfaces.Repositories;
using Blog.Domain.DTOs.Post;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Posts.Commands;

public class PostgresPostCommandRepository(PostgresContext context) : IPostRepositories.IPostgresCommand
{
    public async Task<int[]> BulkCreate(
        List<CreatePostDto> posts,
        CancellationToken cancellationToken = default)
    {
        var postDtos = posts.ToList();
        if (postDtos.Count == 0)
            return [];

        var sql = new StringBuilder(
            "INSERT INTO t_posts (author_id, title, content, status, tags) VALUES ");

        var values = new List<string>();
        foreach (var post in postDtos)
        {
            var authorId = post.AuthorId;
            var title = post.Title.Replace("'", "''");
            var content = post.Content.Replace("'", "''");
            var status = (int)post.Status;
            var tags = post.Tags.Replace("'", "''");

            values.Add($"('{authorId}', '{title}', '{content}', {status}, '{tags}')");
        }

        sql.Append(string.Join(", ", values));
        sql.Append(" RETURNING id;");

        var createdIds = await context.Database
            .SqlQueryRaw<int>(sql.ToString())
            .ToArrayAsync(cancellationToken);

        return createdIds;
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

    public async Task BulkUpdate(List<UpdatePostDto> posts, CancellationToken cancellationToken)
    {
        var sql = new StringBuilder("UPDATE t_posts SET ");

        var updateFields = new List<string>();

        if (posts.Any(p => p.Title is not null))
            updateFields.Add("title = CASE id " +
                             string.Join(" ", posts.Where(p => p.Title is not null)
                                 .Select(p => $"WHEN {p.Id} THEN '{p.Title!.Replace("'", "''")}'")) +
                             " END");

        if (posts.Any(p => p.Content is not null))
            updateFields.Add("content = CASE id " +
                             string.Join(" ", posts.Where(p => p.Content is not null)
                                 .Select(p => $"WHEN {p.Id} THEN '{p.Content!.Replace("'", "''")}'")) +
                             " END");

        if (posts.Any(p => p.Status.HasValue))
            updateFields.Add("status = CASE id " +
                             string.Join(" ", posts.Where(p => p.Status.HasValue)
                                 .Select(p => $"WHEN {p.Id} THEN {p.Status!.Value}")) +
                             " END");

        if (posts.Any(p => p.Tags is not null))
            updateFields.Add("tags = CASE id " +
                             string.Join(" ", posts.Where(p => p.Tags is not null)
                                 .Select(p => $"WHEN {p.Id} THEN '{p.Tags!.Replace("'", "''")}'")) +
                             " END");

        if (updateFields.Count == 0)
            return;

        sql.Append(string.Join(", ", updateFields));
        sql.Append($" WHERE id IN ({string.Join(", ", posts.Select(p => p.Id))});");

        await context.Database.ExecuteSqlRawAsync(sql.ToString(), cancellationToken);
    }
}