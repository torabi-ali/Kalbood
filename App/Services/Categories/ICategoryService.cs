using App.ViewModels.Categories;
using App.ViewModels.Home;
using Data;

namespace App.Services.Categories;

public interface ICategoryService
{
    Task<CategoryDetailDto> GetByIdAsync(int id);

    Task<CategoryDetailDto> GetByUrlAsync(string url);

    Task<IPagedList<CategoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPinned = false);

    Task<List<SitemapNode>> GetSitemapNodeAsync();

    Task<int> InsertAsync(CategoryCreateDto input);

    Task<int> UpdateAsync(int id, CategoryEditDto input);

    Task<int> DeleteAsync(int id);

    Task<CategoryCreateDto> PrepareModelAsync();

    Task<CategoryEditDto> PrepareModelAsync(int id);
}