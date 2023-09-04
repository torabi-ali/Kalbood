using App.ViewModels.Common;

namespace App.ViewModels.Faqs;

public record FaqListDto : BaseViewModel
{
    public string Url { get; set; }

    public string Question { get; set; }

    public string Answer { get; set; }

    public int DisplayOrder { get; set; }
}
