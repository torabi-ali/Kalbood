using System.ComponentModel.DataAnnotations;
using App.ViewModels.Common;

namespace App.ViewModels.Faqs;

public record FaqDetailDto : BaseViewModel
{
    [Required]
    public string Url { get; set; }

    [Required]
    public string Question { get; set; }

    [Required]
    public string Answer { get; set; }

    public int DisplayOrder { get; set; }
}
