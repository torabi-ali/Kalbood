using App.Services.Urls;
using App.ViewModels.Urls;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Controllers;

public class UrlHistoriesController(IUrlHistoryService urlHistoryService) : Controller
{
    [Route("/{*url}", Order = 999)]
    [OutputCache(NoStore = true)]
    public async Task<IActionResult> CatchAll(string url)
    {
        var trimmedUrl = $"/{url.ToLowerInvariant().TrimStart('/')}";
        var urlHistory = await urlHistoryService.GetByUrlAsync(trimmedUrl);

        if (urlHistory is null)
        {
            return NotFound();
        }

        if (urlHistory.NewUrl is null)
        {
            return new StatusCodeResult(urlHistory.HttpStatus);
        }

        return urlHistory.HttpStatus switch
        {
            301 => new RedirectResult(urlHistory.NewUrl, permanent: false, preserveMethod: false),
            302 => new RedirectResult(urlHistory.NewUrl, permanent: true, preserveMethod: false),
            307 => new RedirectResult(urlHistory.NewUrl, permanent: false, preserveMethod: true),
            308 => new RedirectResult(urlHistory.NewUrl, permanent: true, preserveMethod: true),
            _ => throw new ArgumentException("Error while redirecting"),
        };
    }

    [Route("Error/{code:int}")]
    [OutputCache(NoStore = true)]
    public IActionResult Error(int code)
    {
        var message = code switch
        {
            int n when n is < 400 and >= 300 => "خطا در انتقال شما به صفحه جدید رخ داده است",
            int n when n is < 500 and >= 400 => "صفحه مورد نظر شما یافت نشد",
            int n when n is < 600 and >= 500 => "خطا فنی رخ داده است",
            _ => "خطایی در سیستم رخ داده است",
        };

        var model = new ErrorDto
        {
            Message = message
        };

        return View(model);
    }
}
