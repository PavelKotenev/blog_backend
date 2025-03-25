using System.Text.Json;
using Blog.Contracts.Responses;

namespace Blog.Infrastructure.Services;

public class ElasticResponseMapper
{
    public static async Task<CountPostsByCategoriesResponse> MapToCountPostsByCategoriesResponse(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(json);
        var aggregations = doc.RootElement.GetProperty("aggregations");

        var totalByContent = 0;
        var totalByTitle = 0;
        var totalByTag = 0;
        var totalById = 0;

        if (aggregations.TryGetProperty("by_contents", out var byContentElement))
        {
            totalByContent = byContentElement.GetProperty("doc_count").GetInt32();
        }

        if (aggregations.TryGetProperty("by_titles", out var byTitleElement))
        {
            totalByTitle = byTitleElement.GetProperty("doc_count").GetInt32();
        }

        if (aggregations.TryGetProperty("by_tags", out var byTagElement))
        {
            totalByTag = byTagElement.GetProperty("doc_count").GetInt32();
        }

        if (aggregations.TryGetProperty("by_ids", out var byIdElement))
        {
            totalById = byIdElement.GetProperty("doc_count").GetInt32();
        }

        return new CountPostsByCategoriesResponse(totalById, totalByContent, totalByTitle, totalByTag);
    }


    public static async Task<GetPostsByCategoryResponse> MapToPostsByCategoryResponse(
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(json);
        var rootElement = doc.RootElement;

        var total = rootElement.GetProperty("hits")
            .GetProperty("total")
            .GetProperty("value")
            .GetInt32();

        var posts = MapToPostsByCategory(rootElement, cancellationToken);

        return new GetPostsByCategoryResponse(
            Total: total,
            PostsByCategory: posts
        );
    }

    private static List<PreviewPost> MapToPostsByCategory(
        JsonElement rootElement,
        CancellationToken cancellationToken
    )
    {
        return rootElement.GetProperty("hits")
            .GetProperty("hits")
            .EnumerateArray()
            .Select(hit =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var source = hit.GetProperty("_source");

                return new PreviewPost(
                    Id: source.GetProperty("id").GetInt64(),
                    Title: source.GetProperty("title").GetString() ?? string.Empty,
                    Content: source.GetProperty("content").GetString() ?? string.Empty,
                    Tags: MapToPreviewPostTags(source.GetProperty("tags").GetString() ?? string.Empty),
                    CreatedAt: source.GetProperty("created_at").GetInt64()
                );
            })
            .ToList();
    }

    private static List<PreviewPostTag> MapToPreviewPostTags(string tagsString)
    {
        if (string.IsNullOrEmpty(tagsString))
        {
            return new List<PreviewPostTag>();
        }

        return tagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(tagPair =>
            {
                var parts = tagPair.Split("$$", StringSplitOptions.RemoveEmptyEntries);
                return parts.Length == 2 && int.TryParse(parts[0], out var tagId)
                    ? new PreviewPostTag(tagId, parts[1])
                    : null;
            })
            .Where(tag => tag != null)
            .ToList()!;
    }
}