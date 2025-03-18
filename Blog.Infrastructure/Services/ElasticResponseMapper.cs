using System.Text.Json;
using Blog.Contracts.Responses;

namespace Blog.Infrastructure.Services;

public class ElasticResponseMapper
{
    public static async Task<GetAllCategoriesPostsResponse> MapToAllCatgoriesPostsResponse(
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(json);

        try
        {
            var total = doc.RootElement.GetProperty("hits").GetProperty("total").GetProperty("value").GetInt32();

            var aggregations = doc.RootElement.GetProperty("aggregations").EnumerateObject()
                .Select(agg =>
                {
                    var category = agg.Name;
                    var totalCount = agg.Value
                        .GetProperty("top_posts")
                        .GetProperty("hits")
                        .GetProperty("total")
                        .GetProperty("value")
                        .GetInt32();

                    var posts = ParsePreviewPosts(agg.Value.GetProperty("top_posts"), cancellationToken);

                    return new PostsCategoryAggregation(category, totalCount, posts);
                })
                .ToList();

            return new GetAllCategoriesPostsResponse(total, aggregations);
        }
        catch (KeyNotFoundException)
        {
            return new GetAllCategoriesPostsResponse(0, []);
        }
    }


    public static async Task<GetPostsByCategoryResponse> MapToPostsByCategoryResponse(
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(json);

        var posts = ParsePreviewPosts(doc.RootElement, cancellationToken);

        return new GetPostsByCategoryResponse(posts);
    }


    private static List<PreviewPost> ParsePreviewPosts(
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
        return tagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(tagPair =>
            {
                var parts = tagPair.Split("$$");
                return parts.Length == 2 && int.TryParse(parts[0], out var tagId)
                    ? new PreviewPostTag(tagId, parts[1])
                    : null;
            })
            .Where(tag => tag != null)
            .ToList()!;
    }
}