using App.ViewModels.Home;
using App.ViewModels.Posts;
using Data;

namespace App.Services.Posts;

public interface IPostService
{
    Task<PostDetailDto> GetByIdAsync(int id);

    Task<PostDetailDto> GetByUrlAsync(string url);

    Task<IPagedList<PostListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPublished = false, bool onlyPinned = false);

    Task<List<SitemapNode>> GetSitemapNodeAsync();

    Task<int> InsertAsync(PostCreateDto input);

    Task<int> UpdateAsync(int id, PostEditDto input);

    Task<int> DeleteAsync(int id);

    Task<PostCreateDto> PrepareModelAsync();

    Task<PostEditDto> PrepareModelAsync(int id);
}
