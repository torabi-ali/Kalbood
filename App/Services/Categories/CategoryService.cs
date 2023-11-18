using App.ViewModels.Categories;
using App.ViewModels.Common;
using App.ViewModels.Home;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Categories;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Categories;

public class CategoryService(KalboodDbContext dbContext, IMapper mapper) : ICategoryService
{
    public Task<CategoryDetailDto> GetByIdAsync(int id)
    {
        return dbContext.Set<Category>().Where(p => p.Id == id).ProjectTo<CategoryDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<CategoryDetailDto> GetByUrlAsync(string url)
    {
        return dbContext.Set<Category>().Where(p => p.Url.Equals(url, StringComparison.Ordinal)).ProjectTo<CategoryDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<IPagedList<CategoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPinned = false)
    {
        var query = dbContext.Set<Category>().AsQueryable();

        if (onlyPinned)
        {
            query = query.OrderByDescending(p => p.IsPin).ThenByDescending(p => p.CreatedOn);
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedOn);
        }

        return query.ProjectTo<CategoryListDto>(mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public Task<List<SitemapNode>> GetSitemapNodeAsync()
    {
        return dbContext.Set<Category>()
                 .ProjectTo<SitemapNode>(mapper.ConfigurationProvider)
                 .ToListAsync();
    }

    public async Task<int> InsertAsync(CategoryCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = mapper.Map<Category>(input);
        dbContext.Set<Category>().Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, CategoryEditDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = await dbContext.Set<Category>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = mapper.Map(input, entity);
        dbContext.Set<Category>().Update(entity);

        return await dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Category { Id = id };
        dbContext.Set<Category>().Remove(entity);

        return dbContext.SaveChangesAsync();
    }

    public async Task<CategoryCreateDto> PrepareModelAsync()
    {
        var parentCategories = await dbContext.Set<Category>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();

        return new CategoryCreateDto
        {
            ParentCategories = parentCategories
        };
    }

    public async Task<CategoryEditDto> PrepareModelAsync(int id)
    {
        var parentCategories = await dbContext.Set<Category>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();
        var result = await dbContext.Set<Category>().Where(p => p.Id == id).ProjectTo<CategoryEditDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.ParentCategories = parentCategories;

        return result;
    }
}
