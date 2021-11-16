using System;
using System.Net.Http;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using SFA.DAS.Employer.FrontDoor.Contentful.GdsHtmlRenderers;
using SFA.DAS.Employer.FrontDoor.Contentful.TestHarness.Model;

namespace SFA.DAS.Employer.FrontDoor.Contentful.TestHarness
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var client = new ContentfulClient(httpClient,
                "",
                "",
                "0liyzri8haz6");

            //todo: get all
            var entry = await client.GetEntry<Scheme>("6YMOVJcUS66vdhyP4q9CAs");

            var htmlRenderer = new HtmlRenderer();
            htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
            string description = await htmlRenderer.ToHtml(entry.Description);
            string detailsPageOverride = await htmlRenderer.ToHtml(entry.DetailsPageOverride);
        }
    }
}
