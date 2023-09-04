using App.ViewModels.Common;
using Data.Utility;

namespace App.ViewModels.Posts;

public record PostListDto : BaseViewModel
{
    public bool IsPublished { get; set; }

    public bool IsPin { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string Text { get; set; }

    public string Summary => StringUtility.Summary(Text);

    public int Views { get; set; }

    public int Likes { get; set; }

    public string ImageUrl { get; set; }
}