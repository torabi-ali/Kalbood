using App.ViewModels.Common;
using App.ViewModels.Posts;

namespace App.ViewModels.Categories;

public record CategoryDetailDto : ContentViewModel
{
    public int? ParentId { get; set; }

    public string ParentTtile { get; set; }

    public IEnumerable<PostListDto> Posts { get; set; }
}
