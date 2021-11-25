using System;
using System.Linq;
using System.Net.Http;
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

            var builder = QueryBuilder<Scheme>.New.ContentTypeIs("scheme");

            var schemes = await client.GetEntries<Scheme>(builder);

            // ensure we order by size desc, so we don't have to sort at run time
            var schemesBiggestFirst = schemes.OrderByDescending(s => s.Size);

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

            foreach (Scheme scheme in schemesBiggestFirst)
            {
                Console.WriteLine($"new Scheme(\"{scheme.Name}\",");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortDescription, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortCost, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortBenefits, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortTime, htmlRenderer)},");
                Console.WriteLine($"\"{scheme.Url}\", {scheme.Size}, {await AsHtmlString(scheme.DetailsPageOverride, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Description, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Cost, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Responsibility, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Benefits, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.CaseStudies, htmlRenderer)},");
                Console.WriteLine($"\"{scheme.OfferHeader}\",");
                Console.WriteLine($"{await AsHtmlString(scheme.Offer, htmlRenderer)}),");
            }
        }
        //todo: empty strings as nulls?
        
        private static async Task<string> AsHtmlString(Document? document, HtmlRenderer htmlRenderer)
        {
            if (document == null)
                return "null";

            string unescapedHtml = await htmlRenderer.ToHtml(document);
            string html = unescapedHtml.Replace("\"", "\"\"");
            // sometimes contentful uses a \r and sometimes a \r\n - nice!
            // we could strip these out instead
            html = html.Replace("\r\n", "\r");
            html = html.Replace("\r", "\r\n");

            return $"new HtmlString(@\"{html}\")";
        }
    }
}
