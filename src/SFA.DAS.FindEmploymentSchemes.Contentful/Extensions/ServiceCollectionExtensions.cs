using Contentful.AspNetCore;
using Contentful.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContentService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //todo: hmm, this uses a singleton HttpClient, so will we hit dns issues that IHttpClientFactory fixes??
            //todo: add extension to add support for ContentService
            serviceCollection.AddContentful(configuration);
            // from looking at the code, appears to be thread safe (although the invoke intervals should be far enough apart so that it doesn't have to be)
            //services.AddSingleton<HtmlRenderer>();
            //services.AddSingleton(sp => )
            //services.AddSingleton<IGdsHtmlRenderer, GdsHtmlRenderer>();

            serviceCollection.AddTransient(sp =>
            {
                var htmlRendererOptions = new HtmlRendererOptions
                {
                    ListItemOptions =
                    {
                        OmitParagraphTagsInsideListItems = true
                    }
                };
                var htmlRenderer = new HtmlRenderer(htmlRendererOptions);
                htmlRenderer.AddRenderer(new GdsCtaContentRenderer(htmlRenderer.Renderers));
                htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
                htmlRenderer.AddRenderer(new GdsHorizontalRulerContentRenderer());
                htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
                htmlRenderer.AddRenderer(new GdsListContentRenderer(htmlRenderer.Renderers));
                htmlRenderer.AddRenderer(new GdsParagraphRenderer(htmlRenderer.Renderers));

                return htmlRenderer;
            });

            serviceCollection.AddTransient<IContentService, ContentService>();

            return serviceCollection;
        }
    }
}