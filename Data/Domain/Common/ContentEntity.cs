namespace Data.Domain.Common;

public abstract class ContentEntity : BaseEntity, IContentEntity
{
    public string Title { get; set; }

    public string Url { get; set; }

    public string Text { get; set; }

    public string ImageUrl { get; set; }
}