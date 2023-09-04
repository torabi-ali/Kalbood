using App.ViewModels.Common;
using App.ViewModels.Home;
using App.ViewModels.Posts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Categories;
using Data.Domain.Posts;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Posts;

public class PostService : IPostService
{
    private readonly KalboodDbContext _dbContext;
    private readonly IMapper _mapper;

    public PostService(KalboodDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<PostDetailDto> GetByIdAsync(int id)
    {
        return _dbContext.Set<Post>().Where(p => p.Id == id).ProjectTo<PostDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<PostDetailDto> GetByUrlAsync(string url)
    {
        return _dbContext.Set<Post>()
            .Where(p => p.IsPublished)
            .Where(p => p.Url.Equals(url))
            .ProjectTo<PostDetailDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public Task<IPagedList<PostListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPublished = false, bool onlyPinned = false)
    {
        var query = _dbContext.Set<Post>().AsQueryable();

        if (onlyPublished)
        {
            query = query.Where(p => p.IsPublished);
        }

        if (onlyPinned)
        {
            query = query.OrderByDescending(p => p.IsPin).ThenByDescending(p => p.CreatedOn);
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedOn);
        }

        return query.ProjectTo<PostListDto>(_mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public Task<List<SitemapNode>> GetSitemapNodeAsync()
    {
        return _dbContext.Set<Post>()
            .Where(p => p.IsPublished)
            .ProjectTo<SitemapNode>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<int> InsertAsync(PostCreateDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = _mapper.Map<Post>(input);
        await UpdateCategoriesAsync(entity, input.SelectedCategories);
        _dbContext.Set<Post>().Add(entity);

        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, PostEditDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = await _dbContext.Set<Post>().Where(p => p.Id == id).Include(p => p.Categories).SingleOrDefaultAsync();
        entity = _mapper.Map(input, entity);
        await UpdateCategoriesAsync(entity, input.SelectedCategories);
        _dbContext.Set<Post>().Update(entity);

        return await _dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Post { Id = id };
        _dbContext.Set<Post>().Remove(entity);

        return _dbContext.SaveChangesAsync();
    }

    public async Task<PostCreateDto> PrepareModelAsync()
    {
        var categories = await _dbContext.Set<Category>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();

        return new PostCreateDto
        {
            Categories = categories
        };
    }

    public async Task<PostEditDto> PrepareModelAsync(int id)
    {
        var categories = await _dbContext.Set<Category>().ProjectTo<SelectViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        var result = await _dbContext.Set<Post>().Where(p => p.Id == id).ProjectTo<PostEditDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.Categories = categories;

        return result;
    }

    private async Task UpdateCategoriesAsync(Post post, IEnumerable<int> newCategories)
    {
        if (post.Categories is null)
        {
            post.Categories = new List<PostCategory>();
        }

        var oldCategories = post.Categories.Select(c => c.CategoryId).ToList();
        var toDelete = oldCategories.Except(newCategories).ToList();
        var toAdd = newCategories.Except(oldCategories).ToList();

        var toDeleteItems = post.Categories.Where(p => toDelete.Contains(p.CategoryId)).ToList();
        foreach (var item in toDeleteItems)
        {
            post.Categories.Remove(item);
        }

        var toAddItems = await _dbContext.Set<Category>()
                                         .Where(p => toAdd.Contains(p.Id))
                                         .Select(p => new PostCategory
                                         {
                                             CategoryId = p.Id,
                                             PostId = post.Id
                                         }).ToListAsync();
        foreach (var item in toAddItems)
        {
            post.Categories.Add(item);
        }
    }
}