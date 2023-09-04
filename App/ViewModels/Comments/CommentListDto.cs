using App.ViewModels.Common;

namespace App.ViewModels.Comments;

public record CommentListDto : BaseViewModel
{
    public string Title { get; set; }

    public string Url { get; set; }

    public int ViewCount { get; set; }
}
