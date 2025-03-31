using System.Text.Json;
using Blog.Domain.DTOs.Post;
using Blog.Domain.Enums;
using Bogus;

namespace Blog.Infrastructure.Services;

public class FakerService
{
    private static readonly Random Random = new();

    public static List<CreatePostDto> CreateFakePosts(int count)
    {
        var faker = new Faker<CreatePostDto>("en")
            .RuleFor(p => p.AuthorId, f => 1)
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(1))
            .RuleFor(p => p.Status, f => ActivityStatus.Active)
            .RuleFor(p => p.Tags, f => GenerateRandomTagsJson());

        return Enumerable.Range(0, count)
            .Select(_ => faker.Generate())
            .ToList();
    }

    private static string GenerateRandomTagsJson()
    {
        var tagIds = new HashSet<int>();

        while (tagIds.Count < Random.Next(1, 6))
        {
            tagIds.Add(Random.Next(1, 13));
        }

        return JsonSerializer.Serialize(tagIds);
    }
}