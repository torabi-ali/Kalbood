namespace App.Services.Home;

public interface ISitemapService
{
    Task<string> PrepareSitemapModelAsync();
}
