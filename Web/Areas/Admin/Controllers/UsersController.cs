using App.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class UsersController(IUserService userService) : BaseAdminController
{
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20, string roleName = null)
    {
        var model = await userService.GetAllPagedAsync(pageIndex, pageSize, roleName);

        return View(model);
    }
}
