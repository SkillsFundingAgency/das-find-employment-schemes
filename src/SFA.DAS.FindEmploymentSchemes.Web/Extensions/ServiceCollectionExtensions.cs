
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Infrastructure;
using SFA.DAS.FindEmploymentSchemes.Web.Logging;


namespace SFA.DAS.FindEmploymentSchemes.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNLog(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var nLogConfiguration = new NLogConfiguration();

            serviceCollection.AddLogging((options) =>
            {
                options.AddFilter(typeof(Startup).Namespace, LogLevel.Information);
                options.SetMinimumLevel(LogLevel.Trace);
                options.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
                options.AddConsole();

                nLogConfiguration.ConfigureNLog(configuration["NLog:LogLevel"]);
            });

            return serviceCollection;
        }

        public static IServiceCollection GenerateSitemap(this IServiceCollection serviceCollection, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (Uri.TryCreate(configuration["Endpoints:BaseURL"], UriKind.Absolute, out Uri? baseUri))
                Sitemap.Generate(env, baseUri);
            return serviceCollection;
        }
    }
}
