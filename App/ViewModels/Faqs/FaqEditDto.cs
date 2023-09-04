using App.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Faqs;

public record FaqEditDto : BaseViewModel
{
    [Required]
    public string Url { get; set; }

    [Required]
    public string Question { get; set; }

    [Required]
    public string Answer { get; set; }

    public int DisplayOrder { get; set; }
}