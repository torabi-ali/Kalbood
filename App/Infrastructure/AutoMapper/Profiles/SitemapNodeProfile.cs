using App.ViewModels.Home;
using AutoMapper;
using Data.Domain.Categories;
using Data.Domain.Posts;

namespace App.Infrastructure.AutoMapper.Profiles;

public class SitemapNodeProfile : Profile
{
    public SitemapNodeProfile()
    {
        CreateMap<Category, SitemapNode>()
            .ForMember(d => d.Url, s => s.MapFrom(p => $"categories/{p.Url}/"))
            .ForMember(d => d.Priority, s => s.MapFrom(p => 0.85))
            .ForMember(d => d.Frequency, s => s.MapFrom(p => SitemapFrequency.Daily))
            .ForMember(d => d.LastModified, s => s.MapFrom(p => DateTime.Today));

        CreateMap<Post, SitemapNode>()
            .ForMember(d => d.Url, s => s.MapFrom(p => $"posts/{p.Url}/"))
            .ForMember(d => d.Priority, s => s.MapFrom(p => 1.0))
            .ForMember(d => d.Frequency, s => s.MapFrom(p => SitemapFrequency.Daily))
            .ForMember(d => d.LastModified, s => s.MapFrom(p => DateTime.Today));
    }
}
