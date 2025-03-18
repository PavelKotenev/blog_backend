using System.Text.RegularExpressions;

namespace Blog.Infrastructure.Services;

public static partial class QueryStringAnalyzer
{
    public static bool IsSearchByIdAvailable(string queryString)
    {
        return IsSearchByIdAvailableRegex().IsMatch(queryString);
    }

    public static List<int> ExtractIds(string queryString)
    {
        return ExtractIdsRegex().Matches(queryString)
            .Select(m => int.Parse(m.Value))
            .OrderBy(n => n)
            .ToList();
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex IsSearchByIdAvailableRegex();

    [GeneratedRegex("\\d+")]
    private static partial Regex ExtractIdsRegex();
}