using App.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Controllers;

public class PostsController : Controller
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 20)
    {
        var data = await _postService.GetAllPagedAsync(pageIndex, pageSize, onlyPublished: true);
        return View(data);
    }

    [Route("{controller}/{url}")]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public async Task<ActionResult> Detail(string url)
    {
        var model = await _postService.GetByUrlAsync(url);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
}