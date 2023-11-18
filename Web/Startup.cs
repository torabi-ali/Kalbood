using App.Infrastructure.Startup;
using Data.DbContext;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Web;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<KalboodDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddDataProtection().PersistKeysToDbContext<KalboodDbContext>();

        services.AddControllersWithViews();

        services.AddRazorPages();

        #region Identity

        services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<KalboodDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = true;
        });

        #endregion

        services.RegisterServices();

        services.RegisterMapping();

        services.AddHttpContextAccessor();

        services.AddRouting(option =>
        {
            option.LowercaseUrls = true;
            option.AppendTrailingSlash = true;
        });

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.AddWebEncoders(o =>
        {
            o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
        });

        services.AddOutputCache(options =>
        {
            options.AddPolicy("ExpireIn3000s", builder => builder.Expire(TimeSpan.FromSeconds(3000)));
            options.AddPolicy("ExpireIn300s", builder => builder.Expire(TimeSpan.FromSeconds(300)));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseResponseCompression();

        app.UseStaticFiles(new StaticFileOptions
        {
            HttpsCompression = HttpsCompressionMode.Compress,
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
            }
        });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseOutputCache();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(name: "area", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();
        });
    }
}
