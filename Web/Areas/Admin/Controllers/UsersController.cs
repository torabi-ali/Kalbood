using App.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class UsersController : BaseAdminController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20, string roleName = null)
    {
        var model = await _userService.GetAllPagedAsync(pageIndex, pageSize, roleName);

        return View(model);
    }
}
