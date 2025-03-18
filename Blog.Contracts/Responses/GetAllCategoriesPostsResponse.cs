using System.Text.Json.Serialization;

namespace Blog.Contracts.Responses;

public record GetAllCategoriesPostsResponse(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("aggregations")]
    List<PostsCategoryAggregation> Aggregations
);

public record PostsCategoryAggregation(
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("posts")] List<PreviewPost> Posts
);
