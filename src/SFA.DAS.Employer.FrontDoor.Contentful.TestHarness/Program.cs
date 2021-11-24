using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
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

            //var entry = await client.GetEntry<Scheme>("6YMOVJcUS66vdhyP4q9CAs");


            var builder = QueryBuilder<Scheme>.New.ContentTypeIs("scheme");

            var entries = await client.GetEntries<Scheme>(builder);

            var htmlRenderer = new HtmlRenderer();
            htmlRenderer.AddRenderer(new GdsCtaContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHorizontalRulerContentRenderer());
            htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsListContentRenderer(htmlRenderer.Renderers));

            var content = new StringBuilder();
            foreach (Scheme entry in entries)
            {
                content.AppendLine($"new Scheme(\"{entry.Name}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.ShortDescription, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.ShortCost, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.ShortBenefits, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.ShortTime, htmlRenderer)}\",");
                content.AppendLine($"\"{entry.Url}\", 0, \"{await AsHtmlString(entry.DetailsPageOverride, htmlRenderer)}\"");
                content.AppendLine($"\"{await AsHtmlString(entry.Description, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.Cost, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.Responsibility, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.Benefits, htmlRenderer)}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.CaseStudies, htmlRenderer)}\",");
                content.AppendLine($"\"{entry.OfferHeader}\",");
                content.AppendLine($"\"{await AsHtmlString(entry.Offer, htmlRenderer)}\"),");
            }

            Console.WriteLine(content.ToString());
        }

        private static async Task<string> AsHtmlString(Document? document, HtmlRenderer htmlRenderer)
        {
            if (document == null)
                return "new HtmlString(\"\")";

            return $"new HtmlString(@\"{(await htmlRenderer.ToHtml(document)).Replace("\"", "\"\"")}\")";
        }
    }
}
