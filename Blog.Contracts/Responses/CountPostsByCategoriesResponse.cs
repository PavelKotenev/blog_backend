using System.Text.Json.Serialization;

namespace Blog.Contracts.Responses;

public record CountPostsByCategoriesResponse(
    [property: JsonPropertyName("totalByIds")] int TotalByIds,
    [property: JsonPropertyName("totalByContents")] int TotalByContents,
    [property: JsonPropertyName("totalByTitles")] int TotalByTitles,
    [property: JsonPropertyName("totalByTags")] int TotalByTags
);
