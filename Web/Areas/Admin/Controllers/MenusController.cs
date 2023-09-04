using App.Services.Menus;
using App.ViewModels.Menus;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class MenusController : BaseAdminController
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await _menuService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public async Task<IActionResult> Create()
    {
        var model = await _menuService.PrepareModelAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(MenuCreateDto menuAction)
    {
        if (ModelState.IsValid)
        {
            await _menuService.InsertAsync(menuAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await _menuService.PrepareModelAsync();
        return View(model);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await _menuService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, MenuEditDto menuAction)
    {
        if (ModelState.IsValid)
        {
            await _menuService.UpdateAsync(id, menuAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await _menuService.PrepareModelAsync(id);
        return View(model);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await _menuService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}