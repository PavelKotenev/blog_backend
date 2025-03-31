using System.Text.Json.Serialization;
using Blog.Domain.DTOs.MvTagsStatistics;

namespace Blog.Contracts.Responses;

public record GetAllTagsResponse(
    [property: JsonPropertyName("all_tags")]
    List<MvTagsStatisticsDto> SuggestedTagsBatch
);