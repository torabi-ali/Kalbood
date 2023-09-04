using App.ViewModels.Faqs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.DbContext;
using Data.Domain.Faqs;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Faqs;

public class FaqService : IFaqService
{
    private readonly KalboodDbContext _dbContext;
    private readonly IMapper _mapper;

    public FaqService(KalboodDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<FaqDetailDto> GetByIdAsync(int id)
    {
        return _dbContext.Set<Faq>().Where(p => p.Id == id).ProjectTo<FaqDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public Task<List<FaqDetailDto>> GetByUrlAsync(string url)
    {
        return _dbContext.Set<Faq>()
            .Where(p => p.Url.Equals(url))
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedOn)
            .ProjectTo<FaqDetailDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public Task<IPagedList<FaqListDto>> GetAllPagedAsync(int pageIndex, int pageSize)
    {
        var query = _dbContext.Set<Faq>().OrderByDescending(p => p.CreatedOn);

        return query.ProjectTo<FaqListDto>(_mapper.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<int> InsertAsync(FaqCreateDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = _mapper.Map<Faq>(input);
        _dbContext.Set<Faq>().Add(entity);

        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<int> UpdateAsync(int id, FaqEditDto input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var entity = await _dbContext.Set<Faq>().Where(p => p.Id == id).SingleOrDefaultAsync();
        entity = _mapper.Map(input, entity);
        _dbContext.Set<Faq>().Update(entity);

        return await _dbContext.SaveChangesAsync();
    }

    public Task<int> DeleteAsync(int id)
    {
        var entity = new Faq { Id = id };
        _dbContext.Set<Faq>().Remove(entity);

        return _dbContext.SaveChangesAsync();
    }

    public async Task<FaqEditDto> PrepareModelAsync(int id)
    {
        return await _dbContext.Set<Faq>().Where(p => p.Id == id).ProjectTo<FaqEditDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}
