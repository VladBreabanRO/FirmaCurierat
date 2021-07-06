using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FirmaCurierat.Data;
using Radzen;
using System.Net.Http;
using Syncfusion.Blazor;

namespace FirmaCurierat
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        partial void OnConfigureServices(IServiceCollection services);

        partial void OnConfiguringServices(IServiceCollection services);

        public void ConfigureServices(IServiceCollection services)
        {
            OnConfiguringServices(services);

            services.AddHttpContextAccessor();
            services.AddScoped<HttpClient>(serviceProvider =>
            {

                var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();

                return new HttpClient
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            services.AddHttpClient();
        //    services.AddScoped<RlvMailerService>();
         //   services.AddDbContext<RlvMailer.Data.RlvMailerContext>(ServiceLifetime.Transient);
            //(options =>
            //{
            //  options.UseSqlServer(); // Configuration.GetConnectionString("RLVMailerConnection")
            //});

            services.AddRazorPages();
            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(opt => { opt.DetailedErrors = true; });
            services.AddSyncfusionBlazor();
            services.AddAntDesign();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddLocalization();


            var supportedCultures = new[]
            {
                new System.Globalization.CultureInfo("ro-RO"),
                new System.Globalization.CultureInfo("fr-FR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("de-DE"),
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            OnConfigureServices(services);
        }

        partial void OnConfigure(IApplicationBuilder app, IWebHostEnvironment env);
        partial void OnConfiguring(IApplicationBuilder app, IWebHostEnvironment env);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzMyMjY3QDMxMzgyZTMzMmUzMGxPc2RSRFREU1Fxa2craVIrQ1gyQUlEWjhsM1ZRcVYwMGpNZ05ocDBVSGs9");
            OnConfiguring(app, env);

            var supportedCultures = new[]
            {
                new System.Globalization.CultureInfo("ro-RO"),
                new System.Globalization.CultureInfo("fr-FR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("de-DE"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use((ctx, next) =>
                {
                    return next();
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
       
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            OnConfigure(app, env);
        }
    }
}
