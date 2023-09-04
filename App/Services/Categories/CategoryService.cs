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

public class CategoryService : ICategoryService
{
    private readonly KalboodDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(KalboodDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<CategoryDetailDto> GetByIdAsync(int id)
    {
        return _dbContext.Set<Category>().Where(p => p.Id == id).ProjectTo<CategoryDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<CategoryDetailDto> GetByUrlAsync(string url)
    {
        return _dbContext.Set<Category>().Where(p => p.Url.Equals(url)).ProjectTo<CategoryDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<IPagedList<CategoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPinned = false)
    {
        var query = _dbContext.Set<Category>().AsQueryable();

        if (onlyPinned)
        {
            query = query.OrderByDescending(p => p.IsPin).ThenByDescending(p => p.CreatedOn);
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedOn);
        }

        return query.ProjectTo<CategoryListDto>(_mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public Task<List<SitemapNode>> GetSitemapNodeAsync()
    {
        return _dbContext.Set<Category>()
                 .ProjectTo<SitemapNode>(_mapper.ConfigurationProvider)
                 .ToListAsync();
    }

    public async Task<int> InsertAsync(CategoryCreateDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = _mapper.Map<Category>(input);
        _dbContext.Set<Category>().Add(entity);

        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, CategoryEditDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = await _dbContext.Set<Category>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = _mapper.Map(input, entity);
        _dbContext.Set<Category>().Update(entity);

        return await _dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Category { Id = id };
        _dbContext.Set<Category>().Remove(entity);

        return _dbContext.SaveChangesAsync();
    }

    public async Task<CategoryCreateDto> PrepareModelAsync()
    {
        var parentCategories = await _dbContext.Set<Category>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();

        return new CategoryCreateDto
        {
            ParentCategories = parentCategories
        };
    }

    public async Task<CategoryEditDto> PrepareModelAsync(int id)
    {
        var parentCategories = await _dbContext.Set<Category>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        var result = await _dbContext.Set<Category>().Where(p => p.Id == id).ProjectTo<CategoryEditDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.ParentCategories = parentCategories;

        return result;
    }
}
