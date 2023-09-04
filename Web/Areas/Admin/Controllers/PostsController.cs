using App.Services.Posts;
using App.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;
using Web.Extensions;

namespace Web.Areas.Admin.Controllers;

public class PostsController : BaseAdminController
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await _postService.GetAllPagedAsync(pageIndex, pageSize);
        return View(data);
    }

    public async Task<IActionResult> Create()
    {
        var model = await _postService.PrepareModelAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PostCreateDto postAction, IFormFile image)
    {
        if (string.IsNullOrEmpty(postAction.ImageUrl) && image is null)
        {
            ModelState.AddModelError("Image", "Image can not be empty.");
        }

        if (ModelState.IsValid)
        {
            postAction.ImageUrl = await image.UploadFileAsync("posts", postAction.Title);

            await _postService.InsertAsync(postAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await _postService.PrepareModelAsync();
        return View(model);
    }

    public async Task<ActionResult> Edit(int id)
    {
        var model = await _postService.PrepareModelAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, PostEditDto postAction, IFormFile image)
    {
        if (string.IsNullOrEmpty(postAction.ImageUrl) && image is null)
        {
            ModelState.AddModelError("Image", "Image can not be empty.");
        }

        if (ModelState.IsValid)
        {
            if (image is not null)
            {
                postAction.ImageUrl = await image.UploadFileAsync("posts", postAction.Title);
            }

            await _postService.UpdateAsync(id, postAction);
            return RedirectToAction(nameof(Index));
        }

        var model = await _postService.PrepareModelAsync(id);
        return View(model);
    }

    public async Task<ActionResult> Delete(int id)
    {
        await _postService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}