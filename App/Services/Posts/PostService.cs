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

public class PostService(KalboodDbContext dbContext, IMapper mapper) : IPostService
{
    public Task<PostDetailDto> GetByIdAsync(int id)
    {
        return dbContext.Set<Post>().Where(p => p.Id == id).ProjectTo<PostDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<PostDetailDto> GetByUrlAsync(string url)
    {
        return dbContext.Set<Post>()
            .Where(p => p.IsPublished)
            .Where(p => p.Url.Equals(url, StringComparison.Ordinal))
            .ProjectTo<PostDetailDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public Task<IPagedList<PostListDto>> GetAllPagedAsync(int pageIndex, int pageSize, bool onlyPublished = false, bool onlyPinned = false)
    {
        var query = dbContext.Set<Post>().AsQueryable();

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

        return query.ProjectTo<PostListDto>(mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public Task<List<SitemapNode>> GetSitemapNodeAsync()
    {
        return dbContext.Set<Post>()
            .Where(p => p.IsPublished)
            .ProjectTo<SitemapNode>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<int> InsertAsync(PostCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = mapper.Map<Post>(input);
        await UpdateCategoriesAsync(entity, input.SelectedCategories);
        dbContext.Set<Post>().Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, PostEditDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = await dbContext.Set<Post>().Where(p => p.Id == id).Include(p => p.Categories).SingleOrDefaultAsync();
        entity = mapper.Map(input, entity);
        await UpdateCategoriesAsync(entity, input.SelectedCategories);
        dbContext.Set<Post>().Update(entity);

        return await dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Post { Id = id };
        dbContext.Set<Post>().Remove(entity);

        return dbContext.SaveChangesAsync();
    }

    public async Task<PostCreateDto> PrepareModelAsync()
    {
        var categories = await dbContext.Set<Category>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();

        return new PostCreateDto
        {
            Categories = categories
        };
    }

    public async Task<PostEditDto> PrepareModelAsync(int id)
    {
        var categories = await dbContext.Set<Category>().ProjectTo<SelectViewModel>(mapper.ConfigurationProvider).ToListAsync();
        var result = await dbContext.Set<Post>().Where(p => p.Id == id).ProjectTo<PostEditDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        result.Categories = categories;

        return result;
    }

    private async Task UpdateCategoriesAsync(Post post, IEnumerable<int> newCategories)
    {
        post.Categories ??= new List<PostCategory>();

        var oldCategories = post.Categories.Select(c => c.CategoryId).ToList();
        var toDelete = oldCategories.Except(newCategories).ToList();
        var toAdd = newCategories.Except(oldCategories).ToList();

        var toDeleteItems = post.Categories.Where(p => toDelete.Contains(p.CategoryId)).ToList();
        foreach (var item in toDeleteItems)
        {
            post.Categories.Remove(item);
        }

        var toAddItems = await dbContext.Set<Category>()
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
