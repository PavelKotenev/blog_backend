using System.Text.Json.Serialization;
using Blog.Contracts.Responses;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.CountPostsByCategories;

public record CountPostsByCategoriesQuery(
    [property: JsonPropertyName("searchTerm")]
    string SearchTerm,
    [property: JsonPropertyName("fromCreatedAt")]
    long? FromCreatedAt,
    [property: JsonPropertyName("toCreatedAt")]
    long? ToCreatedAt,
    [property: JsonPropertyName("selectedTags")]
    int[]? SelectedTags
) : IRequest<CountPostsByCategoriesResponse>;