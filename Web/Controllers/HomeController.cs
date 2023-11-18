using App.Services.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Controllers;

public class HomeController(IHomePageService homeService, ISitemapService sitemapService) : Controller
{
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IActionResult> Index()
    {
        var model = await homeService.PrepareHomeModelAsync();
        return View(model);
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    [Route("sitemap.xml")]
    public async Task<IActionResult> Sitemap()
    {
        var result = await sitemapService.PrepareSitemapModelAsync();

        return new ContentResult
        {
            Content = result,
            ContentType = "application/xml; charset=utf-8",
            StatusCode = 200
        };
    }
}
