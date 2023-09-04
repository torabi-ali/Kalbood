using App.ViewModels.Categories;
using App.ViewModels.Posts;

namespace App.ViewModels.Home;

public record HomePageDto
{
    public IEnumerable<CategoryListDto> PinnedCategories { get; set; }

    public IEnumerable<PostListDto> PinnedPosts { get; set; }

    public IEnumerable<PostListDto> NewPosts { get; set; }
}
