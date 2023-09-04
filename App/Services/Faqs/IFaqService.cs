using App.ViewModels.Faqs;
using Data;

namespace App.Services.Faqs;

public interface IFaqService
{
    Task<FaqDetailDto> GetByIdAsync(int id);

    Task<List<FaqDetailDto>> GetByUrlAsync(string url);

    Task<IPagedList<FaqListDto>> GetAllPagedAsync(int pageIndex, int pageSize);

    Task<int> InsertAsync(FaqCreateDto input);

    Task<int> UpdateAsync(int id, FaqEditDto input);

    Task<int> DeleteAsync(int id);

    Task<FaqEditDto> PrepareModelAsync(int id);
}
