using App.ViewModels.Menus;
using Data;

namespace App.Services.Menus;

public interface IMenuService
{
    Task<MenuDetailDto> GetByIdAsync(int id);

    Task<IPagedList<MenuListDto>> GetAllPagedAsync(int pageIndex, int pageSize);

    Task<int> InsertAsync(MenuCreateDto input);

    Task<int> UpdateAsync(int id, MenuEditDto input);

    Task<int> DeleteAsync(int id);

    Task<MenuCreateDto> PrepareModelAsync();

    Task<MenuEditDto> PrepareModelAsync(int id);
}