namespace App.ViewModels.Comments;

public record CommentActionDto
{
    public string Text { get; set; }

    public int Likes { get; set; }

    public int PostId { get; set; }
}
