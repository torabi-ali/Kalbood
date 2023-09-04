using App.Services.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await _categoryService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    [Route("{controller}/{url}")]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public async Task<ActionResult> Detail(string url)
    {
        var category = await _categoryService.GetByUrlAsync(url);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
}
