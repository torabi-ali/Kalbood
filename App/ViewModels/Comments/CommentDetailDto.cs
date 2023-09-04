using App.ViewModels.Common;

namespace App.ViewModels.Comments;

public record CommentDetailDto : BaseViewModel
{
    public string Text { get; set; }

    public int Likes { get; set; }

    public int PostId { get; set; }
}
