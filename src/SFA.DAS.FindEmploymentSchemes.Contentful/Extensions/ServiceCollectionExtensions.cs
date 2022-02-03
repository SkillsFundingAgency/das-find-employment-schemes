﻿using System.Net.Http;
using Contentful.AspNetCore;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            // have factory that injects ienumerable<IContentfulClient>, and has client and previewclient properties,
            //using IContentfulClient.IsPreviewClient

            //todo: missing options
            //serviceCollection.AddTransient<IContentfulClient>((sp) => {
            //    var options = sp.GetService<IOptions<ContentfulOptions>>()?.Value;
            //    options!.UsePreviewApi = true;
            //    var client = sp.GetService<HttpClient>();
            //    return new ContentfulClient(client, options);
        //});

            return serviceCollection;
        }
    }
}