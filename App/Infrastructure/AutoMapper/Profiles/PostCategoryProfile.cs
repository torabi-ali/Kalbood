using App.ViewModels.Categories;
using App.ViewModels.Common;
using AutoMapper;
using Data.Domain.Categories;

namespace App.Infrastructure.AutoMapper.Profiles;

public class PostCategoryProfile : Profile
{
    public PostCategoryProfile()
    {
        CreateMap<PostCategory, CategoryListDto>()
            .ForMember(d => d.Id, s => s.MapFrom(p => p.Category.Id))
            .ForMember(d => d.Title, s => s.MapFrom(p => p.Category.Title))
            .ForMember(d => d.Url, s => s.MapFrom(p => p.Category.Url))
            .ForMember(d => d.ParentTitle, s => s.MapFrom(p => p.Category.Parent.Title));

        CreateMap<PostCategory, SelectViewModel>()
            .ForMember(d => d.Value, s => s.MapFrom(p => p.Category.Id))
            .ForMember(d => d.Text, s => s.MapFrom(p => p.Category.Title));
    }
}