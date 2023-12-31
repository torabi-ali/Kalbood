using App.Services.Categories;
using App.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;
using Web.Extensions;

namespace Web.Areas.Admin.Controllers;

public class CategoriesController(ICategoryService categoryService) : BaseAdminController
{
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await categoryService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public async Task<IActionResult> Create()
    {
        var model = await categoryService.PrepareModelAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CategoryCreateDto categoryAction, IFormFile image)
    {
        if (string.IsNullOrEmpty(categoryAction.ImageUrl) && image is null)
        {
            ModelState.AddModelError("Image", "Image can not be empty.");
        }

        if (ModelState.IsValid)
        {
            categoryAction.ImageUrl = await image.UploadFileAsync("categories", categoryAction.Title);

            await categoryService.InsertAsync(categoryAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await categoryService.PrepareModelAsync();
        return View(model);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await categoryService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, CategoryEditDto categoryAction, IFormFile image)
    {
        if (string.IsNullOrEmpty(categoryAction.ImageUrl) && image is null)
        {
            ModelState.AddModelError("Image", "Image can not be empty.");
        }

        if (ModelState.IsValid)
        {
            if (image is not null)
            {
                categoryAction.ImageUrl = await image.UploadFileAsync("categories", categoryAction.Title);
            }

            await categoryService.UpdateAsync(id, categoryAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await categoryService.PrepareModelAsync(id);
        return View(model);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
