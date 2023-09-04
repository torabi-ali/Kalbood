using App.ViewModels.Common;

namespace App.ViewModels.Categories;

public record CategoryListDto : ContentViewModel
{
    public bool IsPin { get; set; }

    public string ParentTitle { get; set; }

    public int PostCount { get; set; }
}
