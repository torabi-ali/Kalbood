using App.ViewModels.Urls;
using Data;

namespace App.Services.Urls;

public interface IUrlHistoryService
{
    Task<UrlHistoryDetailDto> GetByIdAsync(int id);

    Task<UrlHistoryDetailDto> GetByUrlAsync(string url);

    Task<IPagedList<UrlHistoryListDto>> GetAllPagedAsync(int pageIndex, int pageSize);

    Task<int> InsertAsync(UrlHistoryCreateDto input);

    Task<int> UpdateAsync(int id, UrlHistoryEditDto input);

    Task<int> DeleteAsync(int id);

    Task<UrlHistoryEditDto> PrepareModelAsync(int id);
}
