using System.Text.Json.Serialization;
using Blog.Domain.DTOs;

namespace Blog.Contracts.Responses;

public record GetPostsByCategoryResponse(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("postsByCategory")]
    List<PreviewPost> PostsByCategory
);

public record PreviewPost(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("content")]
    string Content,
    [property: JsonPropertyName("tags")] List<PreviewPostTag> Tags,
    [property: JsonPropertyName("createdAt")]
    long CreatedAt
);

public record PreviewPostTag(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title
);