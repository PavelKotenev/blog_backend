using System.Text.Json;
using Blog.Domain.DTOs;
using Blog.Domain.Enums;
using Bogus;

namespace Blog.Infrastructure.Services;

public class FakerService
{
    private static readonly Random Random = new();

    public static List<PostDto> CreateFakePosts(int count)
    {
        var faker = new Faker<PostDto>("en")
            .RuleFor(p => p.AuthorId, f => 1)
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(1))
            .RuleFor(p => p.Status, f => ActivityStatus.Active)
            .RuleFor(p => p.Tags, f => GenerateRandomTagsJson())
            .RuleFor(p => p.CreatedAt, f => GenerateRandomEpochTime());

        return Enumerable.Range(0, count)
            .Select(_ => faker.Generate())
            .ToList();
    }

    private static string GenerateRandomTagsJson()
    {
        var tagIds = new HashSet<int>();

        while (tagIds.Count < Random.Next(1, 6))
        {
            tagIds.Add(Random.Next(1, 6));
        }

        return JsonSerializer.Serialize(tagIds);
    }

    private static long GenerateRandomEpochTime()
    {
        var startDate = DateTime.UtcNow.AddMonths(-1);
        var endDate = DateTime.UtcNow;

        var totalMilliseconds = (endDate - startDate).TotalMilliseconds;
        var randomMilliseconds = Random.NextInt64(0, (long)totalMilliseconds);

        return ((DateTimeOffset)startDate.AddMilliseconds(randomMilliseconds)).ToUnixTimeMilliseconds();
    }
}