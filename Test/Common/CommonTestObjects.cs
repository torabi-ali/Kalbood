using App.Infrastructure.AutoMapper.Profiles;
using AutoMapper;
using Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Test.Common;

internal static class CommonTestObjects
{
    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(SitemapNodeProfile)));
        var mapper = config.CreateMapper();

        return mapper;
    }

    public static KalboodDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<KalboodDbContext>().UseInMemoryDatabase(databaseName: $"Kalbood_{DateTime.Now.ToFileTimeUtc}").Options;
        var dbContext = new KalboodDbContext(options);

        return dbContext;
    }
}
