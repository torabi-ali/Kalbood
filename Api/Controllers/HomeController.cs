using App.Services.Home;
using App.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class HomeController : BaseApiController
{
    private readonly IHomePageService _homeService;

    public HomeController(IHomePageService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public Task<HomePageDto> Get()
    {
        return _homeService.PrepareHomeModelAsync();
    }
}