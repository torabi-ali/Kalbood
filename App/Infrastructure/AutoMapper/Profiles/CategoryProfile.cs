using App.ViewModels.Categories;
using App.ViewModels.Common;
using AutoMapper;
using Data.Domain.Categories;
using Data.Utility;

namespace App.Infrastructure.AutoMapper.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateDto, Category>()
            .ForMember(d => d.Text, s => s.MapFrom(p => StringUtility.SanitizeHtmlCharacters(p.Text)))
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.ParentId, s => s.MapFrom(p => p.ParentId > 0 ? p.ParentId : null))
            .ForMember(d => d.CreatedOn, s => s.MapFrom(p => DateTime.Now));

        CreateMap<CategoryEditDto, Category>()
            .ForMember(d => d.Text, s => s.MapFrom(p => StringUtility.SanitizeHtmlCharacters(p.Text)))
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.ParentId, s => s.MapFrom(p => p.ParentId > 0 ? p.ParentId : null))
            .ForMember(d => d.CreatedOn, s => s.Ignore());

        CreateMap<Category, CategoryEditDto>();

        CreateMap<Category, CategoryDetailDto>()
            .ForMember(d => d.Posts, s => s.MapFrom(p => p.Posts.Select(p => p.Post)));

        CreateMap<Category, CategoryListDto>()
            .ForMember(d => d.PostCount, s => s.MapFrom(p => p.Posts.Count()));

        CreateMap<Category, SelectViewModel>()
            .ForMember(d => d.Value, s => s.MapFrom(p => p.Id))
            .ForMember(d => d.Text, s => s.MapFrom(p => p.Title));
    }
}
