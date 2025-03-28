using System.Text.Json.Serialization;
using Blog.Contracts.Responses;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetPostsByCategory;

public record GetPostsByCategoryQuery(
    [property: JsonPropertyName("category")]
    SearchCategories Category,
    [property: JsonPropertyName("searchTerm")]
    string? SearchTerm,
    [property: JsonPropertyName("fromCreatedAt")]
    long? FromCreatedAt,
    [property: JsonPropertyName("toCreatedAt")]
    long? ToCreatedAt,
    [property: JsonPropertyName("lastPostId")]
    int? LastPostId,
    [property: JsonPropertyName("lastPostCreatedAt")]
    long? LastPostCreatedAt,
    [property: JsonPropertyName("selectedTags")]
    int[]? SelectedTags
) : IRequest<GetPostsByCategoryResponse>;