using App.ViewModels.Common;

namespace App.ViewModels.Menus;

public record MenuListDto : BaseViewModel
{
    public string Title { get; set; }

    public string Url { get; set; }

    public int DisplayOrder { get; set; }

    public int? ParentId { get; set; }

    public string ParentTitle { get; set; }

    public IEnumerable<MenuListDto> SubMenus { get; set; }
}
