using App.ViewModels.Categories;
using App.ViewModels.Common;

namespace App.ViewModels.Posts;

public record PostDetailDto : ContentViewModel
{
    public int Views { get; set; }

    public int Likes { get; set; }

    public IEnumerable<CategoryListDto> Categories { get; set; }
}