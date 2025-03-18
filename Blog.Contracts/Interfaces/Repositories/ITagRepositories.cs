using Blog.Contracts.Responses;

namespace Blog.Contracts.Interfaces.Repositories;
public interface ITagRepositories
{
    public interface IPostgresCommand
    {
        public Task CreateMvTagStatistics(CancellationToken cancellationToken);
        public Task Create(string title, CancellationToken cancellationToken);
        public Task CreateDefaultTags(CancellationToken cancellationToken);
        public Task Delete(int[] ids, CancellationToken cancellationToken);
        public Task RefreshMvTagStatistics(CancellationToken cancellationToken);
    }

    public interface IPostgresQuery
    {
        public Task<GetTagsForPickerResponse> GetTagsForPicker(
            string? searchTerm,
            int? lastTagId,
            int? lastTagPopularity,
            int[]? tagsSelectedViaPreviewPosts,
            CancellationToken cancellationToken
        );
    }
}