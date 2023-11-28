using Microsoft.Extensions.Configuration;

namespace App.ViewModels.Settings;

public record ApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        var config = configuration.GetSection("ApplicationSettings");

        var properties = typeof(ApplicationSettings).GetProperties().Where(p => p.CanWrite);
        foreach (var property in properties)
        {
            property.SetValue(this, config[property.Name], null);
        }
    }

    public string Name { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public string Url { get; set; }

    public string Logo { get; set; }

    public string Phone { get; set; }

    public string PhoneLink => $"tel:+98{Phone.Replace(" ", string.Empty).TrimStart('0')}";

    public string Email { get; set; }

    public string EmailLink => $"mailto:{Email.Replace(" ", string.Empty).Replace("[at]", "@")}";

    public string Whatsapp { get; set; }

    public string Instagram { get; set; }

    public string Telegram { get; set; }

    public string GoogleTagManagerId { get; set; }
}
