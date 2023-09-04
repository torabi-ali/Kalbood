using App.Services.Menus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Components;

public class MenuViewComponent : ViewComponent
{
    private readonly IMenuService _menuService;

    public MenuViewComponent(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _menuService.GetAllPagedAsync(1, int.MaxValue);
        return View(model.Data);
    }
}
