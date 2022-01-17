using Contentful.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContentService(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection
                //todo: hmm, this uses a singleton HttpClient, so will we hit dns issues that IHttpClientFactory fixes??
                .AddContentful(configuration)
                .AddTransient(sp => ContentService.CreateHtmlRenderer())
                .AddSingleton<IContentService, ContentService>();

            return serviceCollection;
        }
    }
}