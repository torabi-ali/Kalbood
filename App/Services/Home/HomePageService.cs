using App.Services.Categories;
using App.Services.Posts;
using App.ViewModels.Home;

namespace App.Services.Home;

public class HomePageService(IPostService postService, ICategoryService categoryService) : IHomePageService
{
    public async Task<HomePageDto> PrepareHomeModelAsync()
    {
        var pinnedCategories = await categoryService.GetAllPagedAsync(1, 3, onlyPinned: true);
        var pinnedPosts = await postService.GetAllPagedAsync(1, 3, onlyPublished: true, onlyPinned: true);
        var newPosts = await postService.GetAllPagedAsync(1, 3, onlyPublished: true);

        return new HomePageDto
        {
            PinnedCategories = pinnedCategories.Data,
            PinnedPosts = pinnedPosts.Data,
            NewPosts = newPosts.Data
        };
    }
}
