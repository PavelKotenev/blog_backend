using System.Text.Json.Serialization;
using Blog.Contracts.Responses;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetPostsByCategory;

public record GetPostsByCategoryQuery(
    [property: JsonPropertyName("category")]
    string Category,
    [property: JsonPropertyName("searchTerm")]
    string? SearchTerm,
    [property: JsonPropertyName("fromCreatedAt")]
    long? FromCreatedAt,
    [property: JsonPropertyName("toCreatedAt")]
    long? ToCreatedAt,
    [property: JsonPropertyName("lastPostId")]
    long? LastPostId,
    [property: JsonPropertyName("lastPostCreatedAt")]
    long? LastPostCreatedAt,
    [property: JsonPropertyName("selectedTags")]
    int[]? SelectedTags
) : IRequest<GetPostsByCategoryResponse>;