namespace App.ViewModels.Settings;

public interface IApplicationSettings
{
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