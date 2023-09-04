using App.ViewModels.Faqs;
using AutoMapper;
using Data.Domain.Faqs;
using Data.Utility;

namespace App.Infrastructure.AutoMapper.Profiles;

public class FaqProfile : Profile
{
    public FaqProfile()
    {
        CreateMap<FaqCreateDto, Faq>()
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.CreatedOn, s => s.MapFrom(p => DateTime.Now));

        CreateMap<FaqEditDto, Faq>()
            .ForMember(d => d.CreatedOn, s => s.Ignore())
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)));

        CreateMap<Faq, FaqEditDto>();

        CreateMap<Faq, FaqDetailDto>();

        CreateMap<Faq, FaqListDto>();
    }
}
