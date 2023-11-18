using System.ComponentModel.DataAnnotations;
using Data.Utility;

namespace App.ViewModels.Common;

public abstract record ContentViewModel : BaseViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    [DataType(DataType.Html)]
    public string Text { get; set; }

    public string Summary => StringUtility.Summary(Text);

    [DataType(DataType.Url)]
    public string ImageUrl { get; set; }
}