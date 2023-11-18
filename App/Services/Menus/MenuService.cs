using App.ViewModels.Common;
using App.ViewModels.Menus;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Menus;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Menus;

public class MenuService(KalboodDbContext dbContext, IMapper mapper) : IMenuService
{
    public Task<MenuDetailDto> GetByIdAsync(int id)
    {
        return dbContext.Set<Menu>().Where(p => p.Id == id).ProjectTo<MenuDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<IPagedList<MenuListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = dbContext.Set<Menu>().Where(p => p.ParentId == null).OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreatedOn);

        return query.ProjectTo<MenuListDto>(mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(MenuCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = mapper.Map<Menu>(input);
        dbContext.Set<Menu>().Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, MenuEditDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = await dbContext.Set<Menu>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = mapper.Map(input, entity);
        dbContext.Set<Menu>().Update(entity);

        return await dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Menu { Id = id };
        dbContext.Set<Menu>().Remove(entity);

        return dbContext.SaveChangesAsync();
    }

    public async Task<MenuCreateDto> PrepareModelAsync()
    {
        var parentMenus = await dbContext.Set<Menu>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();

        return new MenuCreateDto
        {
            ParentMenus = parentMenus
        };
    }

    public async Task<MenuEditDto> PrepareModelAsync(int id)
    {
        var parentMenus = await dbContext.Set<Menu>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();
        var result = await dbContext.Set<Menu>().Where(p => p.Id == id).ProjectTo<MenuEditDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.ParentMenus = parentMenus;

        return result;
    }
}
