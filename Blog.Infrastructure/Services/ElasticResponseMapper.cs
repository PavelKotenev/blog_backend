using System.Text.Json;
using Blog.Domain.Responses;

namespace Blog.Infrastructure.Services;

public class ElasticResponseMapper
{
    public static async Task<SearchAllCategoriesResponse> MapToSuggestions(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        try
        {
            var total = doc.RootElement.GetProperty("hits").GetProperty("total").GetProperty("value").GetInt32();
            var aggregations = doc.RootElement.GetProperty("aggregations").EnumerateObject()
                .ToDictionary(
                    agg => agg.Name,
                    agg =>
                    {
                        var category = agg.Name;
                        var totalCount = agg.Value.GetProperty("top_posts").GetProperty("hits")
                            .GetProperty("total").GetProperty("value").GetInt32();

                        var posts = agg.Value.GetProperty("top_posts").GetProperty("hits").GetProperty("hits")
                            .EnumerateArray()
                            .Select(hit =>
                            {
                                var source = hit.GetProperty("_source");

                                var tagsString = source.GetProperty("tags").GetString() ?? string.Empty;
                                var tagsList = MapToPreviewPostTags(tagsString);

                                return new PreviewPost(
                                    Id: source.GetProperty("id").GetInt64(),
                                    Title: source.GetProperty("title").GetString() ?? string.Empty,
                                    Content: source.GetProperty("content").GetString() ?? string.Empty,
                                    Tags: tagsList,
                                    CreatedAt: source.GetProperty("created_at").GetInt64()
                                );
                            })
                            .ToList();

                        return new PostsCategoryAggregation(category, totalCount, posts);
                    });

            return new SearchAllCategoriesResponse(total, aggregations);
        }
        catch (KeyNotFoundException ex)
        {
            return new SearchAllCategoriesResponse(0, new Dictionary<string, PostsCategoryAggregation>());
        }
    }

    public static async Task<GetPostsByCategoryResponse> MapToFilteredPosts(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var posts = doc.RootElement
            .GetProperty("hits")
            .GetProperty("hits")
            .EnumerateArray()
            .Select(hit =>
            {
                var source = hit.GetProperty("_source");

                var tagsString = source.GetProperty("tags").GetString() ?? string.Empty;
                var tagsList = MapToPreviewPostTags(tagsString);

                return new PreviewPost(
                    Id: source.GetProperty("id").GetInt64(),
                    Title: source.GetProperty("title").GetString() ?? string.Empty,
                    Content: source.GetProperty("content").GetString() ?? string.Empty,
                    Tags: tagsList,
                    CreatedAt: source.GetProperty("created_at").GetInt64()
                );
            })
            .ToList();

        return new GetPostsByCategoryResponse(posts);
    }

    private static List<PreviewPostTag> MapToPreviewPostTags(string tagsString)
    {
        var tagsList = new List<PreviewPostTag>();

        if (!string.IsNullOrEmpty(tagsString))
        {
            var tagPairs = tagsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var tagPair in tagPairs)
            {
                var parts = tagPair.Split("$$");
                if (parts.Length == 2 && int.TryParse(parts[0], out var tagId))
                {
                    tagsList.Add(new PreviewPostTag(tagId, parts[1]));
                }
            }
        }

        return tagsList;
    }
}