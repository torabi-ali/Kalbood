namespace App.ViewModels.Common;

public abstract record BaseViewModel
{
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }
}