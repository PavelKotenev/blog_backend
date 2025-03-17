using System.Text.Json;
using Blog.Domain.DTOs;
using Blog.Domain.Enums;
using Bogus;

namespace Blog.Infrastructure.Services;

public class FakerService
{
    public static List<PostDto> CreateFakePosts(int count)
    {
        var faker = new Faker<PostDto>("en")
            .RuleFor(p => p.AuthorId, f => 1)
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(1))
            .RuleFor(p => p.Status, f => (int)ActivityStatus.Active)
            .RuleFor(p => p.Tags, f => GenerateRandomTagsJson())
            .RuleFor(p => p.CreatedAt, f => GenerateRandomEpochTime());

        var fakePosts = new List<PostDto>();

        for (var i = 0; i < count; i++)
        {
            fakePosts.Add(faker.Generate());
        }

        return fakePosts;
    }

    private static string GenerateRandomTagsJson()
    {
        var random = new Random();
        var tagCount = random.Next(1, 6);
        var tagIds = new HashSet<int>();

        while (tagIds.Count < tagCount)
        {
            var tagId = random.Next(1, 6);
            tagIds.Add(tagId);
        }

        return JsonSerializer.Serialize(tagIds);
    }

    private static long GenerateRandomEpochTime()
    {
        var random = new Random();
        var startDate = DateTime.UtcNow.AddMonths(-1);
        var endDate = DateTime.UtcNow;

        var daysRange = (endDate - startDate).Days;
        var randomDays = random.Next(0, daysRange);

        var randomDate = startDate.AddDays(randomDays);
        
        randomDate = randomDate.AddHours(random.Next(0, 24));
        randomDate = randomDate.AddMinutes(random.Next(0, 60));
        randomDate = randomDate.AddSeconds(random.Next(0, 60));

        return ((DateTimeOffset)randomDate).ToUnixTimeMilliseconds();
    }
}
