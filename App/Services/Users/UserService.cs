using App.ViewModels.Users;
using Data;
using Data.DbContext;
using Data.Extensions;
using Microsoft.AspNetCore.Identity;

namespace App.Services.Users;

public class UserService(KalboodDbContext dbContext) : IUserService
{
    public Task<IPagedList<UserListDto>> GetAllPagedAsync(int pageIndex, int pageSize, string roleName = null)
    {
        var query = from u in dbContext.Set<IdentityUser>()
                    join ur in dbContext.Set<IdentityUserRole<string>>() on u.Id equals ur.UserId into url
                    from ur in url.DefaultIfEmpty()
                    join r in dbContext.Set<IdentityRole>() on ur.RoleId equals r.Id into rl
                    from r in rl.DefaultIfEmpty()
                    select new { User = u, Role = r };

        if (roleName != null)
        {
            query = query.Where(p => p.Role.Name.Equals("Admin"));
        }

        var result = from q in query
                     group q.Role by q.User into grp
                     select new UserListDto
                     {
                         Id = grp.Key.Id,
                         Username = grp.Key.UserName,
                         Email = grp.Key.Email,
                         Roles = string.Join(",", grp.ToList()),
                     };

        return result.ToPagedListAsync(pageIndex, pageSize);
    }
}
