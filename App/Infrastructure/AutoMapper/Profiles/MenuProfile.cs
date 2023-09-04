using App.ViewModels.Common;
using App.ViewModels.Menus;
using AutoMapper;
using Data.Domain.Menus;
using Data.Utility;

namespace App.Infrastructure.AutoMapper.Profiles;

public class MenuProfile : Profile
{
    public MenuProfile()
    {
        CreateMap<MenuCreateDto, Menu>()
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.ParentId, s => s.MapFrom(p => p.ParentId > 0 ? p.ParentId : null))
            .ForMember(d => d.CreatedOn, s => s.MapFrom(p => DateTime.Now));

        CreateMap<MenuEditDto, Menu>()
            .ForMember(d => d.CreatedOn, s => s.Ignore())
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.ParentId, s => s.MapFrom(p => p.ParentId > 0 ? p.ParentId : null));

        CreateMap<Menu, MenuEditDto>();

        CreateMap<Menu, MenuDetailDto>();

        CreateMap<Menu, MenuListDto>();

        CreateMap<Menu, SelectViewModel>()
            .ForMember(d => d.Value, s => s.MapFrom(p => p.Id))
            .ForMember(d => d.Text, s => s.MapFrom(p => p.Title));
    }
}
