using App.ViewModels.Urls;
using AutoMapper;
using Data.Domain.Urls;
using Data.Utility;

namespace App.Infrastructure.AutoMapper.Profiles;

public class UrlProfile : Profile
{
    public UrlProfile()
    {
        CreateMap<UrlHistoryCreateDto, UrlHistory>()
            .ForMember(d => d.OldUrl, s => s.MapFrom(p => StringUtility.TrimUrl(p.OldUrl)))
            .ForMember(d => d.NewUrl, s => s.MapFrom(p => StringUtility.TrimUrl(p.NewUrl)))
            .ForMember(d => d.CreatedOn, s => s.MapFrom(p => DateTime.Now));

        CreateMap<UrlHistoryEditDto, UrlHistory>()
            .ForMember(d => d.OldUrl, s => s.MapFrom(p => StringUtility.TrimUrl(p.OldUrl)))
            .ForMember(d => d.NewUrl, s => s.MapFrom(p => StringUtility.TrimUrl(p.NewUrl)))
            .ForMember(d => d.CreatedOn, s => s.Ignore());

        CreateMap<UrlHistory, UrlHistoryEditDto>();

        CreateMap<UrlHistory, UrlHistoryDetailDto>();

        CreateMap<UrlHistory, UrlHistoryListDto>();
    }
}
