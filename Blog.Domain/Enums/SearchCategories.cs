namespace Blog.Domain.Enums;

public enum SearchCategories
{
    All = 1,
    Id = 2,
    Title = 3,
    Content = 4,
    Tag = 5
}

public static class SearchCategoriesExtensions
{
    public static string GetCategoryName(this SearchCategories category)
    {
        return category switch
        {
            SearchCategories.All => "all",
            SearchCategories.Id => "id",
            SearchCategories.Title => "title",
            SearchCategories.Content => "content",
            SearchCategories.Tag => "tags",
            _ => "unknown"
        };
    }
}