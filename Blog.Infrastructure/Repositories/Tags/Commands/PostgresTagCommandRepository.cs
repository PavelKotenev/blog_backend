using Blog.Contracts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Tags.Commands;

public class PostgresTagCommandRepository(PostgresContext context) : ITagRepositories.IPostgresCommand
{
    public async Task CreateMvTagStatistics(CancellationToken cancellationToken)
    {
        const string query = """

                             CREATE MATERIALIZED VIEW IF NOT EXISTS mv_tag_statistics AS
                             SELECT
                                 t_tags.id,
                                 t_tags.title,
                                 COALESCE(COUNT(tag_id), 0) AS posts_quantity,
                                 COALESCE(DENSE_RANK() OVER (ORDER BY COUNT(tag_id) DESC),
                                          (SELECT DENSE_RANK() OVER (ORDER BY COUNT(*) DESC) FROM t_posts, jsonb_array_elements(tags::jsonb) AS tag_id) + 1) AS popularity
                             FROM t_tags
                                      LEFT JOIN (SELECT tag_id FROM t_posts, jsonb_array_elements(tags::jsonb) AS tag_id) AS tags ON t_tags.id = tags.tag_id::integer
                             GROUP BY t_tags.id
                             ORDER BY popularity;
                             """;

        await context.Database.ExecuteSqlRawAsync(query, cancellationToken);
    }

    public Task Update(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task RefreshMvTagStatistics(CancellationToken cancellationToken)
    {
        await context.Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW mv_tag_statistics;",
            cancellationToken
        );
    }


    public async Task Create(string title, CancellationToken cancellationToken)
    {
        var epochNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var tag = new Domain.Entities.Tag
        {
            Title = title,
            CreatedAt = epochNow
        };
        context.Tag.Add(tag);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(int[] ids, CancellationToken cancellationToken)
    {
        await context.Tag
            .Where(p => ids.Contains(p.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}