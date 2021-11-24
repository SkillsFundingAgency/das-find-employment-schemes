using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
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
            //var entry = await client.GetEntry<Scheme>("6YMOVJcUS66vdhyP4q9CAs");
            var entries = await client.GetEntries<Scheme>();

            var htmlRenderer = new HtmlRenderer();
            htmlRenderer.AddRenderer(new GdsCtaContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHorizontalRulerContentRenderer());
            htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsListContentRenderer(htmlRenderer.Renderers));

            var content = new StringBuilder();
            foreach (Scheme entry in entries)
            {
                content.Append($"new Scheme(\"{entry.Name}\",");
                content.Append($"\"{entry.ShortDescription}\",");
                content.Append($"\"{entry.ShortCost}\",");
                content.Append($"\"{entry.ShortBenefits}\",");
                content.Append($"\"{entry.ShortTime}\",");
                content.Append($"\"{entry.Url}\", 0, \"{await AsHtmlString(entry.DetailsPageOverride, htmlRenderer)}\"");
                content.Append($"\"{await AsHtmlString(entry.Description, htmlRenderer)}\",");
                content.Append($"\"{await AsHtmlString(entry.Cost, htmlRenderer)}\",");
                content.Append($"\"{await AsHtmlString(entry.Responsibility, htmlRenderer)}\",");
                content.Append($"\"{await AsHtmlString(entry.Benefits, htmlRenderer)}\",");
                content.Append($"\"{await AsHtmlString(entry.CaseStudies, htmlRenderer)}\",");
                content.Append($"\"{entry.OfferHeader}\",");
                content.Append($"\"{await AsHtmlString(entry.Offer, htmlRenderer)}\",");
            }
        }

        private static async Task<string> AsHtmlString(Document? document, HtmlRenderer htmlRenderer)
        {
            if (document == null)
                return "new HtmlString(\"\")";

            return $"new HtmlString(@\"{(await htmlRenderer.ToHtml(document)).Replace("\"", "\"\"")}\")";
        }
    }
}
