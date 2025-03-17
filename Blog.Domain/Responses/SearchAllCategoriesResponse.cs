using System.Text.Json.Serialization;

namespace Blog.Domain.Responses;

public record SearchAllCategoriesResponse(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("aggregations")]
    List<PostsCategoryAggregation> Aggregations
);

public record PostsCategoryAggregation(
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("posts")] List<PreviewPost> Posts
);
