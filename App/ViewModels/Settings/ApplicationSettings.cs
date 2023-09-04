using Microsoft.Extensions.Configuration;

namespace App.ViewModels.Settings;

public record ApplicationSettings : IApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        var conf = configuration.GetSection("ApplicationSettings");

        var props = typeof(ApplicationSettings).GetProperties();
        foreach (var prop in props)
        {
            prop.SetValue(this, conf[prop.Name], null);
        }
    }

    public string Name { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public string Url { get; set; }

    public string Logo { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Whatsapp { get; set; }

    public string Instagram { get; set; }

    public string Telegram { get; set; }

    public string GoogleTagManagerId { get; set; }
}