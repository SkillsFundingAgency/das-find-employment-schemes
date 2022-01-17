using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SFA.DAS.FindEmploymentSchemes.Web.Infrastructure;

namespace SFA.DAS.FindEmploymentSchemes.Web.StartupServices
{
    //https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
    public class SitemapGeneratorService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public SitemapGeneratorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();

            var sitemap = scope.ServiceProvider.GetRequiredService<ISitemap>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            //todo: if config missing/incorrect : abort startup (fail fast)

            if (Uri.TryCreate(configuration["Endpoints:BaseURL"], UriKind.Absolute, out Uri? baseUri))
                sitemap.Generate(baseUri);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
