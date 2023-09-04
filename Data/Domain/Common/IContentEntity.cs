namespace Data.Domain.Common;

public interface IContentEntity
{
    public string Title { get; set; }

    public string Url { get; set; }

    public string Text { get; set; }
}