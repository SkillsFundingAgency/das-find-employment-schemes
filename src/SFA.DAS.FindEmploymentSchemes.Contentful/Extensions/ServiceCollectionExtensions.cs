using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Contentful.AspNetCore;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContentService(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                //todo: hmm, this uses a singleton HttpClient, so will we hit dns issues that IHttpClientFactory fixes??
                .AddContentful(configuration)
                .AddTransient(sp => ContentService.CreateHtmlRenderer())
                .AddSingleton<IContentService, ContentService>()
                .AddSingleton<ISchemeService, SchemeService>()
                .AddSingleton<IPageService, PageService>()
                .AddSingleton<IMotivationFilterService, MotivationFilterService>()
                .AddSingleton<ICaseStudyPageService, CaseStudyPageService>()
                .AddSingleton<IPayFilterService, PayFilterService>()
                .AddSingleton<ISchemeLengthFilterService, SchemeLengthFilterService>()
                .AddTransient<IContentfulClient>(sp =>
                {
                    var configOptions = sp.GetService<IOptions<ContentfulOptions>>()?.Value;
                    if (configOptions == null)
                        throw new ConfigurationMissingException("ContentfulOptions");

                    var options = new ContentfulOptions
                    {
                        DeliveryApiKey = configOptions.DeliveryApiKey,
                        Environment = configOptions.Environment,
                        ManagementApiKey = configOptions.ManagementApiKey,
                        MaxNumberOfRateLimitRetries = configOptions.MaxNumberOfRateLimitRetries,
                        PreviewApiKey = configOptions.PreviewApiKey,
                        ResolveEntriesSelectively = configOptions.ResolveEntriesSelectively,
                        SpaceId = configOptions.SpaceId,
                        UsePreviewApi = true
                    };
                    var client = sp.GetService<HttpClient>();
                    return new ContentfulClient(client, options);
                })
                .AddTransient<IContentfulClientFactory, ContentfulClientFactory>();
        }
    }
}