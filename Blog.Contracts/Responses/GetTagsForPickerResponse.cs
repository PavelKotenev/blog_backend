using System.Text.Json.Serialization;
using Blog.Domain.DTOs.MvTagsStatistics;

namespace Blog.Contracts.Responses;

public record GetTagsForPickerResponse(
    [property: JsonPropertyName("selectedTags")]
    List<MvTagsStatisticsDto> SelectedTags,
    [property: JsonPropertyName("suggestedTagsBatch")]
    List<MvTagsStatisticsDto> SuggestedTagsBatch
);