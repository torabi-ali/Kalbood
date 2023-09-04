using App.ViewModels.Common;

namespace App.ViewModels.Posts;

public record PostCreateDto : ContentViewModel
{
    public bool IsPublished { get; set; }

    public bool IsPin { get; set; }

    public int Views { get; set; }

    public int Likes { get; set; }

    public IEnumerable<int> SelectedCategories { get; set; }

    public IEnumerable<SelectViewModel> Categories { get; set; }
}