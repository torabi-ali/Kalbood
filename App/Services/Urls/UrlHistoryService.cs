using App.ViewModels.Urls;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Urls;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Urls;

public class UrlHistoryService : IUrlHistoryService
{
    private readonly KalboodDbContext _dbContext;
    private readonly IMapper _mapper;

    public UrlHistoryService(KalboodDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<UrlHistoryDetailDto> GetByIdAsync(int id)
    {
        return _dbContext.Set<UrlHistory>().Where(p => p.Id == id).ProjectTo<UrlHistoryDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<UrlHistoryDetailDto> GetByUrlAsync(string url)
    {
        return _dbContext.Set<UrlHistory>()
            .Where(p => p.OldUrl.Equals(url))
            .ProjectTo<UrlHistoryDetailDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public Task<IPagedList<UrlHistoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = _dbContext.Set<UrlHistory>().OrderByDescending(p => p.CreatedOn);

        return query.ProjectTo<UrlHistoryListDto>(_mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(UrlHistoryCreateDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = _mapper.Map<UrlHistory>(input);
        _dbContext.Set<UrlHistory>().Add(entity);

        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, UrlHistoryEditDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = await _dbContext.Set<UrlHistory>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = _mapper.Map(input, entity);
        _dbContext.Set<UrlHistory>().Update(entity);

        return await _dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new UrlHistory { Id = id };
        _dbContext.Set<UrlHistory>().Remove(entity);

        return _dbContext.SaveChangesAsync();
    }

    public Task<UrlHistoryEditDto> PrepareModelAsync(int id)
    {
        return _dbContext.Set<UrlHistory>().Where(p => p.Id == id).ProjectTo<UrlHistoryEditDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}
