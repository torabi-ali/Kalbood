using App.ViewModels.Common;
using Data.Utility;

namespace App.ViewModels.Categories;

public record CategoryListDto : BaseViewModel
{
    public bool IsPin { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string Text { get; set; }

    public string Summary => StringUtility.Summary(Text);

    public string ImageUrl { get; set; }

    public string ParentTitle { get; set; }

    public int PostCount { get; set; }
}
