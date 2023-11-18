using System.Reflection;
using App.Infrastructure.Startup;
using Data.DbContext;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Api;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<KalboodDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddDataProtection().PersistKeysToDbContext<KalboodDbContext>();

        services.AddControllers();

        services.RegisterServices();

        services.RegisterMapping();

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.AddOutputCache(options =>
        {
            options.AddPolicy("ExpireIn3000s", builder => builder.Expire(TimeSpan.FromSeconds(3000)));
            options.AddPolicy("ExpireIn300s", builder => builder.Expire(TimeSpan.FromSeconds(300)));
        });

        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kalbood", Version = $"v{Assembly.GetEntryAssembly().GetName().Version}" }));

        services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Kalbood v{Assembly.GetEntryAssembly().GetName().Version}");
                c.DocExpansion(DocExpansion.None);
            });
        }

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors();

        app.UseOutputCache();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
