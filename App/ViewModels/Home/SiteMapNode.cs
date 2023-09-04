namespace App.ViewModels.Home;

public record SitemapNode
{
    public SitemapFrequency Frequency { get; set; }

    public DateTime LastModified { get; set; }

    public double Priority { get; set; }

    public string Url { get; set; }
}

public enum SitemapFrequency
{
    Never,
    Yearly,
    Monthly,
    Weekly,
    Daily,
    Hourly,
    Always
}
