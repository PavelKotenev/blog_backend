using Blog.Domain.MaterializedView;
using Blog.Domain.Responses;

namespace Blog.Domain.Interfaces.Repositories;

public interface ITagRepositories
{
    public interface IPostgresCommand
    {
        public Task CreateMvTagStatistics();
        public Task Create(string title);
        public Task CreateDefaultTags();
        public Task Delete(int[] ids);
        public Task RefreshMvTagStatistics();
    }

    public interface IPostgresQuery
    {
        public Task<GetTagsForPickerResponse> GetTagsPickerTags(
            int? lastTagId,
            int? lastTagPopularity,
            int[] tagsSelectedViaPreviewPosts
        );
    }
}