using App.Services.Menus;
using App.ViewModels.Menus;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class MenusController(IMenuService menuService) : BaseAdminController
{
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await menuService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public async Task<IActionResult> Create()
    {
        var model = await menuService.PrepareModelAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(MenuCreateDto menuAction)
    {
        if (ModelState.IsValid)
        {
            await menuService.InsertAsync(menuAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await menuService.PrepareModelAsync();
        return View(model);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await menuService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, MenuEditDto menuAction)
    {
        if (ModelState.IsValid)
        {
            await menuService.UpdateAsync(id, menuAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await menuService.PrepareModelAsync(id);
        return View(model);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await menuService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
