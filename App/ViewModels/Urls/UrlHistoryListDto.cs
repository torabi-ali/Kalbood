using App.ViewModels.Common;

namespace App.ViewModels.Urls;

public record UrlHistoryListDto : BaseViewModel
{
    public string OldUrl { get; set; }

    public string NewUrl { get; set; }

    public int HttpStatus { get; set; }
}
