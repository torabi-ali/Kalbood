using System.Reflection;
using App.Services.Categories;
using App.Services.Faqs;
using App.Services.Home;
using App.Services.Menus;
using App.Services.Posts;
using App.Services.Urls;
using App.Services.Users;
using App.ViewModels.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Startup;

public static class StartupExtentions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationSettings, ApplicationSettings>();

        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IMenuService, MenuService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IHomePageService, HomePageService>();
        services.AddTransient<ISitemapService, SitemapService>();
        services.AddTransient<IFaqService, FaqService>();
        services.AddTransient<IUrlHistoryService, UrlHistoryService>();
    }

    public static void RegisterMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}