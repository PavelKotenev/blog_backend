using System.Text;
using System.Text.Json;
using Blog.Domain.DTOs;

namespace Blog.Infrastructure.Services;

public static class ElasticQueryBuilder
{
    public static string BuildCreatePostIndexQuery()
    {
        var jsonBody = new
        {
            settings = new
            {
                index = new
                {
                    max_ngram_diff = 8
                },
                analysis = new
                {
                    analyzer = new
                    {
                        ngram_analyzer = new
                        {
                            tokenizer = "ngram_tokenizer"
                        }
                    },
                    tokenizer = new
                    {
                        ngram_tokenizer = new
                        {
                            type = "ngram",
                            min_gram = 3,
                            max_gram = 10
                        }
                    }
                }
            },
            mappings = new
            {
                properties = new
                {
                    id = new { type = "keyword" },
                    title = new { type = "text", analyzer = "ngram_analyzer", search_analyzer = "ngram_analyzer", norms = false },
                    content = new { type = "text", analyzer = "ngram_analyzer", search_analyzer = "ngram_analyzer", norms = false },
                    created_at = new { type = "date", format = "epoch_millis" },
                    tags = new { type = "text", analyzer = "ngram_analyzer", search_analyzer = "ngram_analyzer", norms = false }
                }
            }
        };

        return JsonSerializer.Serialize(jsonBody);
    }
    public static string BuildBulkDeleteDocQuery(int[] docsIds)
    {
        var jsonBody = new
        {
            query = new
            {
                terms = new
                {
                    id = docsIds
                }
            }
        };
        return JsonSerializer.Serialize(jsonBody);
    }

    public static string BuildBulkCreateQuery(List<PostDocumentDto> dtos)
    {
        var sb = new StringBuilder();
        foreach (var dto in dtos)
        {
            var indexOperation = new { index = new { _index = "i_post", _id = dto.Id } };
            sb.AppendLine(JsonSerializer.Serialize(indexOperation));
            sb.AppendLine(JsonSerializer.Serialize(new
            {
                id = dto.Id,
                title = dto.Title,
                content = dto.Content,
                created_at = dto.CreatedAt,
                tags = dto.Tags
            }));
        }

        sb.AppendLine();
        return sb.ToString();
    }

    public static string BuildDeletePostByIdQuery(long id)
    {
        var jsonBody = new Dictionary<string, object>
        {
            ["query"] = new Dictionary<string, object>
            {
                ["term"] = new Dictionary<string, object>
                {
                    ["id"] = new { value = id }
                }
            }
        };

        return JsonSerializer.Serialize(jsonBody);
    }

    public static string BuildPostsByCategoryQuery(
        string category,
        string? searchTerm,
        long? fromCreatedAt,
        long? toCreatedAt,
        long? lastPostId,
        long? lastPostCreatedAt,
        int[]? selectedTags
    )
    {
        var jsonBody = new Dictionary<string, object>
        {
            ["size"] = 20,
            ["sort"] = new List<object>
            {
                new Dictionary<string, object> { ["created_at"] = new { order = "desc" } },
                new Dictionary<string, object> { ["id"] = new { order = "desc" } }
            },
            ["_source"] = new[] { "id", "title", "content", "created_at", "tags" }
        };

        if (lastPostCreatedAt.HasValue && lastPostId.HasValue)
        {
            jsonBody["search_after"] = new List<object> { lastPostCreatedAt.Value, lastPostId.Value };
        }

        var queryFilters = new List<Dictionary<string, object>>();
        var rangeFilter = new Dictionary<string, object>();

        if (fromCreatedAt.HasValue)
        {
            rangeFilter["gte"] = fromCreatedAt.Value;
        }

        if (toCreatedAt.HasValue)
        {
            rangeFilter["lte"] = toCreatedAt.Value;
        }

        if (rangeFilter.Count > 0)
        {
            queryFilters.Add(new Dictionary<string, object>
            {
                ["range"] = new Dictionary<string, object>
                {
                    ["created_at"] = rangeFilter
                }
            });
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var isSearchByIdAvailable = QueryStringAnalyzer.IsSearchByIdAvailable(searchTerm);
            if (category == "id" && isSearchByIdAvailable)
            {
                var ids = QueryStringAnalyzer.ExtractIds(searchTerm);
                queryFilters.Add(new Dictionary<string, object>
                {
                    ["terms"] = new Dictionary<string, object>
                    {
                        ["id"] = ids
                    }
                });
            }
            else
            {
                if (category != "default")
                {
                    queryFilters.Add(new Dictionary<string, object>
                    {
                        ["match"] = new Dictionary<string, object>
                        {
                            [category] = searchTerm
                        }
                    });
                }
            }
        }

        if (selectedTags != null)
        {
            var tagFilters = new List<Dictionary<string, object>>();
            foreach (var tagId in selectedTags)
            {
                tagFilters.Add(new Dictionary<string, object>
                {
                    ["wildcard"] = new Dictionary<string, object>
                    {
                        ["tags"] = new { value = $"*{tagId}$$*" }
                    }
                });
            }

            queryFilters.Add(new Dictionary<string, object>
            {
                ["bool"] = new Dictionary<string, object>
                {
                    ["should"] = tagFilters,
                    ["minimum_should_match"] = 1
                }
            });
        }

        if (queryFilters.Count > 0)
        {
            jsonBody["query"] = new Dictionary<string, object>
            {
                ["bool"] = new Dictionary<string, object>
                {
                    ["must"] = queryFilters
                }
            };
        }

        return JsonSerializer.Serialize(jsonBody);
    }


    public static string BuildCountPostsByCategoryQuery(
        string queryTerm,
        long? fromCreatedAt,
        long? toCreatedAt,
        int[]? selectedTags
    )
    {
        var shouldBlock = new List<object>();
        var aggregationsBlock = new Dictionary<string, object>();
        var queryFilters = new List<Dictionary<string, object>>();

        var isSearchByIdAvailable = QueryStringAnalyzer.IsSearchByIdAvailable(queryTerm);

        var rangeFilter = new Dictionary<string, object>();
        if (fromCreatedAt.HasValue)
        {
            rangeFilter["gte"] = fromCreatedAt.Value;
        }

        if (toCreatedAt.HasValue)
        {
            rangeFilter["lte"] = toCreatedAt.Value;
        }

        if (rangeFilter.Count > 0)
        {
            queryFilters.Add(new Dictionary<string, object>
            {
                ["range"] = new Dictionary<string, object>
                {
                    ["created_at"] = rangeFilter
                }
            });
        }

        if (selectedTags != null && selectedTags.Length > 0)
        {
            var tagFilters = new List<Dictionary<string, object>>();
            foreach (var tagId in selectedTags)
            {
                tagFilters.Add(new Dictionary<string, object>
                {
                    ["wildcard"] = new Dictionary<string, object>
                    {
                        ["tags"] = new { value = $"*{tagId}$$*" }
                    }
                });
            }

            queryFilters.Add(new Dictionary<string, object>
            {
                ["bool"] = new Dictionary<string, object>
                {
                    ["should"] = tagFilters,
                    ["minimum_should_match"] = 1
                }
            });
        }

        shouldBlock.Add(new Dictionary<string, object>
            { ["match"] = new Dictionary<string, object> { ["content"] = queryTerm } });
        aggregationsBlock["by_contents"] = new Dictionary<string, object>
        {
            ["filter"] = new Dictionary<string, object>
            {
                ["match"] = new Dictionary<string, object> { ["content"] = queryTerm }
            }
        };

        shouldBlock.Add(new Dictionary<string, object>
            { ["match"] = new Dictionary<string, object> { ["title"] = queryTerm } });
        aggregationsBlock["by_titles"] = new Dictionary<string, object>
        {
            ["filter"] = new Dictionary<string, object>
            {
                ["match"] = new Dictionary<string, object> { ["title"] = queryTerm }
            }
        };

        shouldBlock.Add(new Dictionary<string, object>
            { ["match"] = new Dictionary<string, object> { ["tags"] = queryTerm } });
        aggregationsBlock["by_tags"] = new Dictionary<string, object>
        {
            ["filter"] = new Dictionary<string, object>
            {
                ["match"] = new Dictionary<string, object> { ["tags"] = queryTerm }
            }
        };

        if (isSearchByIdAvailable)
        {
            var ids = QueryStringAnalyzer.ExtractIds(queryTerm);
            shouldBlock.Add(new Dictionary<string, object>
                { ["terms"] = new Dictionary<string, object> { ["id"] = ids } });
            aggregationsBlock["by_ids"] = new Dictionary<string, object>
            {
                ["filter"] = new Dictionary<string, object>
                {
                    ["terms"] = new Dictionary<string, object> { ["id"] = ids }
                }
            };
        }

        var queryBody = new Dictionary<string, object>
        {
            ["bool"] = new Dictionary<string, object>
            {
                ["should"] = shouldBlock,
                ["minimum_should_match"] = 1
            }
        };

        if (queryFilters.Count > 0)
        {
            queryBody["bool"] = new Dictionary<string, object>
            {
                ["must"] = queryFilters,
                ["should"] = shouldBlock,
                ["minimum_should_match"] = 1
            };
        }

        var jsonBody = new Dictionary<string, object>
        {
            ["size"] = 0,
            ["query"] = queryBody,
            ["aggs"] = aggregationsBlock
        };

        return JsonSerializer.Serialize(jsonBody);
    }
}