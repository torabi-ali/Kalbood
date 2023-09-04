using App.ViewModels.Common;

namespace App.ViewModels.Categories;

public record CategoryCreateDto : ContentViewModel
{
    public bool IsPin { get; set; }

    public int? ParentId { get; set; }

    public IEnumerable<SelectViewModel> ParentCategories { get; set; }
}
