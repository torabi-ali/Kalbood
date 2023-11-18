using App.Services.Posts;
using App.ViewModels.Posts;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class PostsController(IPostService postService) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public Task<IPagedList<PostListDto>> Get(int pageIndex = 0, int pageSize = 20)
    {
        return postService.GetAllPagedAsync(pageIndex, pageSize, onlyPublished: true);
    }

    [HttpGet("{url}")]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public async Task<PostDetailDto> Get(string url)
    {
        var post = await postService.GetByUrlAsync(url) ?? throw new Exception("Not found");

        return post;
    }
}
