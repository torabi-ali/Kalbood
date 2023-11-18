using App.ViewModels.Urls;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Urls;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Urls;

public class UrlHistoryService(KalboodDbContext dbContext, IMapper mapper) : IUrlHistoryService
{
    public Task<UrlHistoryDetailDto> GetByIdAsync(int id)
    {
        return dbContext.Set<UrlHistory>().Where(p => p.Id == id).ProjectTo<UrlHistoryDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<UrlHistoryDetailDto> GetByUrlAsync(string url)
    {
        return dbContext.Set<UrlHistory>()
            .Where(p => p.OldUrl.Equals(url))
            .ProjectTo<UrlHistoryDetailDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public Task<IPagedList<UrlHistoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = dbContext.Set<UrlHistory>().OrderByDescending(p => p.CreatedOn);

        return query.ProjectTo<UrlHistoryListDto>(mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(UrlHistoryCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = mapper.Map<UrlHistory>(input);
        dbContext.Set<UrlHistory>().Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, UrlHistoryEditDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = await dbContext.Set<UrlHistory>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = mapper.Map(input, entity);
        dbContext.Set<UrlHistory>().Update(entity);

        return await dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new UrlHistory { Id = id };
        dbContext.Set<UrlHistory>().Remove(entity);

        return dbContext.SaveChangesAsync();
    }

    public Task<UrlHistoryEditDto> PrepareModelAsync(int id)
    {
        return dbContext.Set<UrlHistory>().Where(p => p.Id == id).ProjectTo<UrlHistoryEditDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}
