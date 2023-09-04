using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class HomeController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }
}
