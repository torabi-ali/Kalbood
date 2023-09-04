using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers.Common;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BaseAdminController : Controller
{ }