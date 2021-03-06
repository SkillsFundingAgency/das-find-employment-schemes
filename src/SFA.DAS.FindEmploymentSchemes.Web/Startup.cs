using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetCore.SEOHelper;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FindEmploymentSchemes.Contentful.Extensions;
using SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices;
using SFA.DAS.FindEmploymentSchemes.Web.Extensions;
using SFA.DAS.FindEmploymentSchemes.Web.Security;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Infrastructure;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.StartupServices;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Routing;

namespace SFA.DAS.FindEmploymentSchemes.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _currentEnvironment = env;

            Configuration = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                })
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNLog(Configuration)
                    .AddHealthChecks();
#if do_we_need_this
            services.AddApplicationInsightsTelemetry();
#endif
#if DEBUG
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
#else
            services.AddControllersWithViews();
#endif
            services.AddWebOptimizer(assetPipeline =>
            {
                if (!_currentEnvironment.IsDevelopment())
                {
                    assetPipeline.AddJavaScriptBundle("/js/site.js",
                        "/js/cookie_consent.js", "/js/show_hide.js", "/js/app.js", "/js/filter.js", "/js/feedback.js");
                    assetPipeline.AddCssBundle("/css/site.css", "/css/site.css");
                }
            });

            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<ICaseStudyPageService, CaseStudyPageService>();

            services.AddSingleton<IFilterService, FilterService>()
                .AddSingleton<ISchemesModelService, SchemesModelService>();

            services.Configure<ContentUpdateServiceOptions>(Configuration.GetSection("ContentUpdates"));

            services.AddContentService(Configuration)
                .AddHostedService<ContentUpdateService>();

            services.Configure<EndpointsOptions>(Configuration.GetSection("Endpoints"));

            services.AddTransient<ISitemap, Sitemap>()
                .AddHostedService<SitemapGeneratorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseAppSecurityHeaders(env, configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });

            app.UseXMLSitemap(env.ContentRootPath);
            app.UseRouting();
            app.UseAuthorization();

            app.UseHealthChecks("/ping", new HealthCheckOptions
            {
                //By returning false to the Predicate option we ensure that none of the health checks registered in ConfigureServices are ran for this endpoint
                Predicate = (_) => false,
                ResponseWriter = (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("whiff-whaff");
                }
            });

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                MapControllerRoute(endpoints,
                    "home",
                    "",
                    "Schemes", "Home");

                MapControllerRoute(endpoints,
                    "page",
                    "page/{pageUrl}",
                    "Pages", "Page");

                MapControllerRoute(endpoints,
                    "casestudypage",
                    "case-study/{pageUrl}",
                    "CaseStudies", "CaseStudyPage");

                MapControllerRoute(endpoints,
                    "schemes",
                    "schemes/{schemeUrl}",
                    "Schemes", "Details");

                MapControllerRoute(endpoints,
                    "scheme-comparison",
                    "scheme-comparison",
                    "Schemes", "Comparison");

                MapControllerRoute(endpoints,
                    "home-preview",
                    "preview/",
                    "Schemes", "HomePreview");

                MapControllerRoute(endpoints,
                    "page-preview",
                    "preview/page/{pageUrl}",
                    "Pages", "PagePreview");

                MapControllerRoute(endpoints,
                    "schemes-preview",
                    "preview/schemes/{schemeUrl}",
                    "Schemes", "DetailsPreview");

                MapControllerRoute(endpoints,
                    "scheme-comparison-preview",
                    "preview/scheme-comparison",
                    "Schemes", "ComparisonPreview");

                MapControllerRoute(endpoints,
                    "casestudypage-preview",
                    "preview/case-study/{pageUrl}",
                    "CaseStudies", "CaseStudyPagePreview");
            });
        }

        /// <remarks>
        /// Work around the over enthusiastic duplicate code quality gate in SonarCloud
        /// </remarks>
        private void MapControllerRoute(IEndpointRouteBuilder builder, string name, string pattern, string controller, string action)
        {
            builder.MapControllerRoute(name, pattern, new { controller, action });
        }
    }
}
