
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.SEOHelper.Sitemap;
using SFA.DAS.FindEmploymentSchemes.Web.Content;


namespace SFA.DAS.FindEmploymentSchemes.Web.Infrastructure
{
    public static class Sitemap
    {
        private static bool done = false;

        public static void Generate(IWebHostEnvironment env, Uri baseUri)
        {
            if (!done)
            {
                List<SitemapNode> nodes = new List<SitemapNode>();

                nodes.AddRange(SchemesContent.Schemes.Select(x => new SitemapNode() {
                    LastModified = DateTime.UtcNow,
                    Priority = 1.0,
                    Frequency = SitemapFrequency.Weekly,
                    Url = new Uri(baseUri, x.Url.StartsWith("/") ? x.Url : $"/{x.Url}").AbsoluteUri
                }));
                nodes.AddRange(SchemesContent.Pages.Select(x => new SitemapNode()
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 1.0,
                    Frequency = SitemapFrequency.Weekly,
                    Url = new Uri(baseUri, x.Url.StartsWith("/") ? x.Url : $"/{x.Url}").AbsoluteUri
                }));

                new SitemapDocument().CreateSitemapXML(nodes, env.ContentRootPath);
                done = true;
            }
        }
    }
}
