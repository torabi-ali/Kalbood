using App.ViewModels.Posts;
using AutoMapper;
using Data.Domain.Posts;
using Data.Utility;

namespace App.Infrastructure.AutoMapper.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<PostCreateDto, Post>()
            .ForMember(d => d.Text, s => s.MapFrom(p => StringUtility.SanitizeHtmlCharacters(p.Text)))
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.Categories, s => s.Ignore())
            .ForMember(d => d.CreatedOn, s => s.MapFrom(p => DateTime.Now));

        CreateMap<PostEditDto, Post>()
            .ForMember(d => d.Text, s => s.MapFrom(p => StringUtility.SanitizeHtmlCharacters(p.Text)))
            .ForMember(d => d.Url, s => s.MapFrom(p => StringUtility.TrimUrl(p.Url)))
            .ForMember(d => d.Categories, s => s.Ignore())
            .ForMember(d => d.CreatedOn, s => s.Ignore());

        CreateMap<Post, PostEditDto>()
            .ForMember(d => d.SelectedCategories, s => s.MapFrom(p => p.Categories.Select(p => p.CategoryId)));

        CreateMap<Post, PostDetailDto>()
            .ForMember(d => d.Categories, s => s.MapFrom(p => p.Categories.Select(p => p.Category)));

        CreateMap<Post, PostListDto>();
    }
}