using App.Services.Faqs;
using App.ViewModels.Faqs;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class FaqsController(IFaqService faqService) : BaseAdminController
{
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await faqService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public IActionResult Create()
    {
        var model = new FaqCreateDto();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(FaqCreateDto faqAction)
    {
        if (ModelState.IsValid)
        {
            await faqService.InsertAsync(faqAction);
            return RedirectToAction(nameof(Index));
        }

        return View(faqAction);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await faqService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, FaqEditDto faqAction)
    {
        if (ModelState.IsValid)
        {
            await faqService.UpdateAsync(id, faqAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await faqService.PrepareModelAsync(id);
        return View(model);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await faqService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
