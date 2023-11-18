using App.Services.Menus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Components;

public class MenuViewComponent(IMenuService menuService) : ViewComponent
{
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await menuService.GetAllPagedAsync(1, int.MaxValue);
        return View(model.Data);
    }
}
