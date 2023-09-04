using Data.Domain.Common;
using Data.Domain.Posts;

namespace Data.Domain.Comments;

public class Comment : BaseEntity
{
    public bool IsPublished { get; set; }

    public string Text { get; set; }

    public int Likes { get; set; }

    public int PostId { get; set; }

    public Post Post { get; set; }
}