using App.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Urls;

public record UrlHistoryCreateDto : BaseViewModel
{
    [Required]
    public string OldUrl { get; set; }

    public string NewUrl { get; set; }

    [Range(100, 600)]
    public int HttpStatus { get; set; }
}
