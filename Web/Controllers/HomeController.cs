using App.Services.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IHomePageService _homeService;
    private readonly ISitemapService _sitemapService;

    public HomeController(IHomePageService homeService, ISitemapService sitemapService)
    {
        _homeService = homeService;
        _sitemapService = sitemapService;
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IActionResult> Index()
    {
        var model = await _homeService.PrepareHomeModelAsync();
        return View(model);
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    [Route("sitemap.xml")]
    public async Task<IActionResult> Sitemap()
    {
        var result = await _sitemapService.PrepareSitemapModelAsync();

        return new ContentResult
        {
            Content = result,
            ContentType = "application/xml; charset=utf-8",
            StatusCode = 200
        };
    }
}