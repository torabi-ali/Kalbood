using App.Services.Urls;
using App.ViewModels.Urls;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class UrlHistoriesController : BaseAdminController
{
    private readonly IUrlHistoryService _urlHistoryService;

    public UrlHistoriesController(IUrlHistoryService urlHistoryService)
    {
        _urlHistoryService = urlHistoryService;
    }

    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await _urlHistoryService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(UrlHistoryCreateDto urlHistoryAction)
    {
        if (ModelState.IsValid)
        {
            await _urlHistoryService.InsertAsync(urlHistoryAction);
            return RedirectToAction(nameof(Index));
        }

        return View(urlHistoryAction);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await _urlHistoryService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, UrlHistoryEditDto urlHistoryAction)
    {
        if (ModelState.IsValid)
        {
            await _urlHistoryService.UpdateAsync(id, urlHistoryAction);
            return RedirectToAction(nameof(Index));
        }

        return View(urlHistoryAction);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await _urlHistoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}