using System.Text.Json.Serialization;
using Blog.Domain.Responses;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetAllCategoriesPosts;

public record GetAllCategoriesPostsQuery(
    [property: JsonPropertyName("searchTerm")]
    string SearchTerm,
    [property: JsonPropertyName("fromCreatedAt")]
    long? FromCreatedAt,
    [property: JsonPropertyName("toCreatedAt")]
    long? ToCreatedAt,
    [property: JsonPropertyName("selectedTags")]
    int[]? SelectedTags
) : IRequest<SearchAllCategoriesResponse>;