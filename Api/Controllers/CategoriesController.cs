using App.Services.Categories;
using App.ViewModels.Categories;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class CategoriesController(ICategoryService categoryService) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public Task<IPagedList<CategoryListDto>> Get(int pageIndex = 0, int pageSize = 20)
    {
        return categoryService.GetAllPagedAsync(pageIndex, pageSize);
    }

    [HttpGet("{url}")]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public async Task<CategoryDetailDto> Get(string url)
    {
        var category = await categoryService.GetByUrlAsync(url) ?? throw new Exception("Not found");

        return category;
    }
}
