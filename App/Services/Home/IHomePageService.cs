using App.ViewModels.Home;

namespace App.Services.Home;

public interface IHomePageService
{
    Task<HomePageDto> PrepareHomeModelAsync();
}
