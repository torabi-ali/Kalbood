using App.Services.Categories;
using App.Services.Posts;
using App.ViewModels.Home;
using App.ViewModels.Settings;
using System.Globalization;
using System.Xml.Linq;

namespace App.Services.Home;

public class SitemapService : ISitemapService
{
    private readonly IPostService _postService;
    private readonly ICategoryService _categoryService;
    private readonly IApplicationSettings _applicationSettings;

    public SitemapService(IPostService postService, ICategoryService categoryService, IApplicationSettings applicationSettings)
    {
        _postService = postService;
        _categoryService = categoryService;
        _applicationSettings = applicationSettings;
    }

    public async Task<string> PrepareSitemapModelAsync()
    {
        var xNamespace = (XNamespace)"http://www.sitemaps.org/schemas/sitemap/0.9";
        var xElement = new XElement(xNamespace + "urlset");

        xElement.Add(CreateElement(xNamespace, string.Empty, DateTime.Today, SitemapFrequency.Always, 1.0)); // Base site address

        var sitemapNodes = new List<SitemapNode>();
        sitemapNodes.AddRange(await _categoryService.GetSitemapNodeAsync());
        sitemapNodes.AddRange(await _postService.GetSitemapNodeAsync());

        foreach (var sitemapNode in sitemapNodes)
        {
            var element = CreateElement(xNamespace, sitemapNode.Url, sitemapNode.LastModified, sitemapNode.Frequency, sitemapNode.Priority);
            xElement.Add(element);
        }

        var declaration = new XDeclaration("1.0", "utf-8", "yes");
        var document = new XDocument(declaration, xElement);
        return document.Declaration.ToString() + Environment.NewLine + document.ToString();
    }

    private XElement CreateElement(XNamespace xNamespace, string objectUrl, DateTime lastModified, SitemapFrequency frequency, double priority)
    {
        var element =
            new XElement(xNamespace + "url",
            new XElement(xNamespace + "loc", new Uri($"{_applicationSettings.Url}{objectUrl}")),
            new XElement(xNamespace + "lastmod", lastModified.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
            new XElement(xNamespace + "changefreq", frequency.ToString().ToLowerInvariant()),
            new XElement(xNamespace + "priority", priority.ToString("F2", CultureInfo.InvariantCulture)));

        return element;
    }
}
