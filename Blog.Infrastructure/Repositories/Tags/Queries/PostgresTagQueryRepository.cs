using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.MaterializedView;
using Blog.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Blog.Infrastructure.Repositories.Tags.Queries;

public class PostgresTagQueryRepository(PostgresContext context) : ITagRepositories.IPostgresQuery
{
    public async Task<GetTagsForPickerResponse> GetTagsPickerTags(
        int? lastTagId,
        int? lastTagPopularity,
        int[] selectedTagIds,
        CancellationToken cancellationToken
    )
    {
        var selectedTagIdsString = selectedTagIds != null ? string.Join(", ", selectedTagIds) : "null";

        var selectedTags = new List<MvTagsStatistics>();
        if (selectedTagIds?.Length > 0)
        {
            const string selectedQuery = "SELECT id, title, posts_quantity, popularity, created_at FROM mv_tag_statistics WHERE id = ANY(:selectedIds)";
            var selectedParameters = new[] { new NpgsqlParameter("selectedIds", selectedTagIds) };
            
            selectedTags = await context.MvTagsStatistics
                .FromSqlRaw(selectedQuery, selectedParameters)
                .ToListAsync(cancellationToken);
        }

        var suggestedQuery = new List<string>();
        var suggestedParameters = new List<NpgsqlParameter>();

        suggestedQuery.Add("SELECT id, title, posts_quantity, popularity, created_at FROM mv_tag_statistics");

        if (lastTagId.HasValue && lastTagPopularity.HasValue)
        {
            suggestedQuery.Add("WHERE (popularity > @p0 OR (popularity = @p0 AND id > @p1))");
            suggestedParameters.Add(new NpgsqlParameter("@p0", lastTagPopularity.Value));
            suggestedParameters.Add(new NpgsqlParameter("@p1", lastTagId.Value));

            if (selectedTagIds?.Length > 0)
            {
                suggestedQuery.Add("AND id != ALL(:excludedIds)");
                suggestedParameters.Add(new NpgsqlParameter("excludedIds", selectedTagIds));
            }
        }
        else if (selectedTagIds?.Length > 0)
        {
            suggestedQuery.Add("WHERE id != ALL(:excludedIds)");
            suggestedParameters.Add(new NpgsqlParameter("excludedIds", selectedTagIds));
        }

        suggestedQuery.Add("ORDER BY popularity ASC, id ASC LIMIT 20");

        var finalSuggestedQuery = string.Join(" ", suggestedQuery);

        var suggestedTagsBatch = await context.MvTagsStatistics
            .FromSqlRaw(finalSuggestedQuery, suggestedParameters.ToArray())
            .ToListAsync(cancellationToken);

        return new GetTagsForPickerResponse(
            SelectedTags: selectedTags,
            SuggestedTagsBatch: suggestedTagsBatch
        );
    }

    public async Task<List<MvTagsStatistics>> GetTagsForAdminTable(int? lastPostId, long? lastPostCreatedAt, CancellationToken cancellationToken)
    {
        var query = "SELECT id, title, posts_quantity, popularity, created_at FROM mv_tag_statistics";

        var parameters = new List<object>();

        if (lastPostId.HasValue && lastPostCreatedAt.HasValue)
        {
            query += " WHERE created_at > @p0 AND id > @p1";
            parameters.Add(lastPostCreatedAt.Value);
            parameters.Add(lastPostId.Value);
        }

        query += " ORDER BY popularity ASC LIMIT 20;";

        var result = await context.MvTagsStatistics
            .FromSqlRaw(query)
            .ToListAsync(cancellationToken);

        return result;
    }
}