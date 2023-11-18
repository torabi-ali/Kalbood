using App.ViewModels.Faqs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Faqs;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Faqs;

public class FaqService(KalboodDbContext dbContext, IMapper mapper) : IFaqService
{
    public Task<FaqDetailDto> GetByIdAsync(int id)
    {
        return dbContext.Set<Faq>().Where(p => p.Id == id).ProjectTo<FaqDetailDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<List<FaqDetailDto>> GetByUrlAsync(string url)
    {
        return dbContext.Set<Faq>()
            .Where(p => p.Url.Equals(url))
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedOn)
            .ProjectTo<FaqDetailDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public Task<IPagedList<FaqListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = dbContext.Set<Faq>().OrderByDescending(p => p.CreatedOn);

        return query.ProjectTo<FaqListDto>(mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(FaqCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = mapper.Map<Faq>(input);
        dbContext.Set<Faq>().Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, FaqEditDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var entity = await dbContext.Set<Faq>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = mapper.Map(input, entity);
        dbContext.Set<Faq>().Update(entity);

        return await dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Faq { Id = id };
        dbContext.Set<Faq>().Remove(entity);

        return dbContext.SaveChangesAsync();
    }

    public async Task<FaqEditDto> PrepareModelAsync(int id)
    {
        return await dbContext.Set<Faq>().Where(p => p.Id == id).ProjectTo<FaqEditDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}
