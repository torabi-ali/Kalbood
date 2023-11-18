using System.ComponentModel.DataAnnotations;
using App.ViewModels.Common;

namespace App.ViewModels.Urls;

public record UrlHistoryEditDto : BaseViewModel
{
    [Required]
    public string OldUrl { get; set; }

    public string NewUrl { get; set; }

    [Range(100, 600)]
    public int HttpStatus { get; set; }
}