using App.Services.Menus;
using App.ViewModels.Menus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class MenusController : BaseApiController
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IEnumerable<MenuListDto>> Get()
    {
        var menus = await _menuService.GetAllPagedAsync(1, int.MaxValue);
        return menus.Data;
    }
}
