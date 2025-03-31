using Blog.Contracts.Interfaces.Repositories;
using Blog.Contracts.Responses;
using Blog.Domain.DTOs.MvTagsStatistics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Blog.Infrastructure.Repositories.Tags.Queries;

public class PostgresTagQueryRepository(PostgresContext context) : ITagRepositories.IPostgresQuery
{
    public async Task<GetTagsForPickerResponse> GetTagsForPicker(
        string? searchTerm,
        int? lastTagId,
        int? lastTagPopularity,
        int[]? selectedTagIds,
        CancellationToken cancellationToken)
    {
        var selectedTags = new List<MvTagsStatisticsDto>();
        var suggestedTagsBatch = new List<MvTagsStatisticsDto>();

        if (selectedTagIds?.Length > 0)
        {
            const string selectedQuery =
                "SELECT id, title, posts_quantity, popularity FROM mv_tag_statistics WHERE id = ANY(:selectedIds)";

            selectedTags = await context.Database
                .SqlQueryRaw<MvTagsStatisticsDto>(selectedQuery, new NpgsqlParameter("selectedIds", selectedTagIds))
                .ToListAsync(cancellationToken);
        }

        var suggestedQuery = new List<string> { "SELECT id, title, posts_quantity, popularity FROM mv_tag_statistics" };
        var suggestedParameters = new List<object>();

        if (lastTagId.HasValue && lastTagPopularity.HasValue)
        {
            suggestedQuery.Add("WHERE (popularity > {0} OR (popularity = {0} AND id > {1}))");
            suggestedParameters.Add(lastTagPopularity.Value);
            suggestedParameters.Add(lastTagId.Value);

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

        suggestedTagsBatch = await context.Database
            .SqlQueryRaw<MvTagsStatisticsDto>(finalSuggestedQuery, suggestedParameters.ToArray())
            .ToListAsync(cancellationToken);

        return new GetTagsForPickerResponse(
            SelectedTags: selectedTags,
            SuggestedTagsBatch: suggestedTagsBatch
        );
    }

    public async Task<List<MvTagsStatisticsDto>> GetTagsForAdminTable(
        int? lastPostId,
        long? lastPostCreatedAt,
        CancellationToken cancellationToken)
    {
        var query = "SELECT id, title, posts_quantity, popularity FROM mv_tag_statistics";
        var parameters = new List<object>();

        if (lastPostId.HasValue && lastPostCreatedAt.HasValue)
        {
            // Предполагаю, что created_at есть в таблице, хотя в DTO его нет
            query += " WHERE created_at > {0} AND id > {1}";
            parameters.Add(lastPostCreatedAt.Value);
            parameters.Add(lastPostId.Value);
        }

        query += " ORDER BY popularity ASC LIMIT 20";

        var result = await context.Database
            .SqlQueryRaw<MvTagsStatisticsDto>(query, parameters.ToArray())
            .ToListAsync(cancellationToken);

        return result;
    }
}