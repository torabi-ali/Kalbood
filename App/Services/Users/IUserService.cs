using App.ViewModels.Users;
using Data;

namespace App.Services.Users;

public interface IUserService
{
    Task<IPagedList<UserListDto>> GetAllPagedAsync(int pageIndex, int pageSize, string roleName = null);
}
