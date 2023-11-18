using System.ComponentModel.DataAnnotations;
using App.ViewModels.Common;

namespace App.ViewModels.Menus;

public record MenuEditDto : BaseViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Url { get; set; }

    public int DisplayOrder { get; set; }

    public int? ParentId { get; set; }

    public IEnumerable<SelectViewModel> ParentMenus { get; set; }
}