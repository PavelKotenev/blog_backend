using System.Text.Json.Serialization;
using Blog.Domain.MaterializedView;

namespace Blog.Contracts.Responses;

public record GetTagsForPickerResponse(
    [property: JsonPropertyName("selectedTags")]
    List<MvTagsStatistics> SelectedTags,
    [property: JsonPropertyName("suggestedTagsBatch")]
    List<MvTagsStatistics> SuggestedTagsBatch
);