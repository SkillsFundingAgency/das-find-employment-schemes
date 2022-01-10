
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
        public static void Generate(IWebHostEnvironment env, Uri baseUri)
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            nodes.AddRange(SchemesContent.Schemes.Select(x => new SitemapNode() {
                Priority = 1.0,
                Frequency = SitemapFrequency.Weekly,
                Url = new Uri(baseUri, string.Concat("schemes", x.Url.StartsWith("/") ? x.Url : $"/{x.Url}"))
                            .AbsoluteUri
            }));
            nodes.AddRange(SchemesContent.Pages.Select(x => new SitemapNode()
            {
                Priority = 1.0,
                Frequency = SitemapFrequency.Weekly,
                Url = new Uri(baseUri, string.Concat("page", x.Url.StartsWith("/") ? x.Url : $"/{x.Url}"))
                            .AbsoluteUri
            }));

            SitemapNode home = nodes.FirstOrDefault(x => x.Url.EndsWith("/home"));
            if (home != null)
                home.Url = home.Url[..^9];

            new SitemapDocument().CreateSitemapXML(nodes, env.ContentRootPath);
        }
    }
}
