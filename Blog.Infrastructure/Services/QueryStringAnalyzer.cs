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

    /// <summary>
    /// Checks if the string meets the specified validation rules.
    /// </summary>
    /// <param name="queryString">The input string to validate.</param>
    /// <returns>true if the string is valid, otherwise false.</returns>
    /// <remarks>
    /// Rules:
    /// 1. The string has 3 or more characters, and the first 3 are letters (e.g., "abc", "abcd").
    /// 2. The string has exactly 3 characters: 2 letters and a space (e.g., "ab ", "a b", " ab").
    /// 3. The string is shorter than 3 characters but starts with "#" (e.g., "#a", "#") or contains at least one digit (e.g., "a1", "1").
    /// </remarks>
    public static bool IsValid(string queryString)
    {
        return Regex.IsMatch(queryString,
            @"^([a-zA-Z]{3,}|([a-zA-Z]{2}\s[a-zA-Z]{1}|\s[a-zA-Z]{2}[a-zA-Z]{1})|#|\d.*)$");
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex IsSearchByIdAvailableRegex();

    [GeneratedRegex("\\d+")]
    private static partial Regex ExtractIdsRegex();
}