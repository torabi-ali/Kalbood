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

public class MenuService : IMenuService
{
    private readonly KalboodDbContext _dbContext;
    private readonly IMapper _mapper;

    public MenuService(KalboodDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<MenuDetailDto> GetByIdAsync(int id)
    {
        return _dbContext.Set<Menu>().Where(p => p.Id == id).ProjectTo<MenuDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<IPagedList<MenuListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = _dbContext.Set<Menu>().Where(p => p.ParentId == null).OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreatedOn);

        return query.ProjectTo<MenuListDto>(_mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(MenuCreateDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = _mapper.Map<Menu>(input);
        _dbContext.Set<Menu>().Add(entity);

        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, MenuEditDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = await _dbContext.Set<Menu>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = _mapper.Map(input, entity);
        _dbContext.Set<Menu>().Update(entity);

        return await _dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Menu { Id = id };
        _dbContext.Set<Menu>().Remove(entity);

        return _dbContext.SaveChangesAsync();
    }

    public async Task<MenuCreateDto> PrepareModelAsync()
    {
        var parentMenus = await _dbContext.Set<Menu>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();

        return new MenuCreateDto
        {
            ParentMenus = parentMenus
        };
    }

    public async Task<MenuEditDto> PrepareModelAsync(int id)
    {
        var parentMenus = await _dbContext.Set<Menu>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        var result = await _dbContext.Set<Menu>().Where(p => p.Id == id).ProjectTo<MenuEditDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.ParentMenus = parentMenus;

        return result;
    }
}
