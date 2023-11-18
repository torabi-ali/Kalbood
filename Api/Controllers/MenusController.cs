using App.Services.Menus;
using App.ViewModels.Menus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class MenusController(IMenuService menuService) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IEnumerable<MenuListDto>> Get()
    {
        var menus = await menuService.GetAllPagedAsync(1, int.MaxValue);
        return menus.Data;
    }
}
