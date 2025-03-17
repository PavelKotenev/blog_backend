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
        int[] selectedTagIds
    )
    {
        string selectedTagIdsString = selectedTagIds != null ? string.Join(", ", selectedTagIds) : "null";
        Console.WriteLine($"Selected Tag IDs: {selectedTagIdsString}");

        // Первый запрос: только для selectedTagIds
        List<MvTagsStatistics> selectedTags = new List<MvTagsStatistics>();
        if (selectedTagIds?.Length > 0)
        {
            var selectedQuery =
                "SELECT id, title, posts_quantity, popularity, created_at FROM mv_tag_statistics WHERE id = ANY(:selectedIds)";
            var selectedParameters = new[] { new NpgsqlParameter("selectedIds", selectedTagIds) };

            var finalSelectedQuery = selectedQuery;
            Console.WriteLine("Selected Tags Query: " + finalSelectedQuery);

            selectedTags = await context.MvTagsStatistics
                .FromSqlRaw(finalSelectedQuery, selectedParameters)
                .ToListAsync();
        }

        // Второй запрос: пагинация без учёта selectedTagIds
        var suggestedQuery = new List<string>();
        var suggestedParameters = new List<NpgsqlParameter>();

        suggestedQuery.Add("SELECT id, title, posts_quantity, popularity, created_at FROM mv_tag_statistics");

        // Условие для пагинации по popularity и id
        if (lastTagId.HasValue && lastTagPopularity.HasValue)
        {
            suggestedQuery.Add("WHERE (popularity > @p0 OR (popularity = @p0 AND id > @p1))");
            suggestedParameters.Add(new NpgsqlParameter("@p0", lastTagPopularity.Value));
            suggestedParameters.Add(new NpgsqlParameter("@p1", lastTagId.Value));

            // Исключаем selectedTagIds из пагинации
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

        // Сортировка и лимит
        suggestedQuery.Add("ORDER BY popularity ASC, id ASC LIMIT 20");

        var finalSuggestedQuery = string.Join(" ", suggestedQuery);
        Console.WriteLine("Suggested Tags Query: " + finalSuggestedQuery);

        var suggestedTagsBatch = await context.MvTagsStatistics
            .FromSqlRaw(finalSuggestedQuery, suggestedParameters.ToArray())
            .ToListAsync();

        // Возвращаем результат
        return new GetTagsForPickerResponse(
            SelectedTags: selectedTags,
            SuggestedTagsBatch: suggestedTagsBatch
        );
    }

    public async Task<List<MvTagsStatistics>> GetTagsForAdminTable(int? lastPostId, long? lastPostCreatedAt)
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
        Console.WriteLine(query);

        var result = await context.MvTagsStatistics
            .FromSqlRaw(query)
            .ToListAsync();

        return result;
    }
}