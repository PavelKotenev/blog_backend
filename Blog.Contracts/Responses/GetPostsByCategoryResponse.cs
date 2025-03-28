using System.Text.Json.Serialization;
using Blog.Domain.DTOs;

namespace Blog.Contracts.Responses;

public record GetPostsByCategoryResponse(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("postsByCategory")]
    List<PostByCategory> PostsByCategory
);

public record PostByCategory(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("content")]
    string Content,
    [property: JsonPropertyName("tags")] List<PostByCategoryTag> Tags,
    [property: JsonPropertyName("createdAt")]
    long CreatedAt
);

public record PostByCategoryTag(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title
);