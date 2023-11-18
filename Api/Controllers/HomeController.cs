using App.Services.Home;
using App.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class HomeController(IHomePageService homeService) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public Task<HomePageDto> Get()
    {
        return homeService.PrepareHomeModelAsync();
    }
}
