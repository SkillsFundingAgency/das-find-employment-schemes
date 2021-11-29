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

            var htmlRenderer = CreateHtmlRenderer();

            await GenerateSchemesContent(client, htmlRenderer);

            await GenerateFilterContent<MotivationsFilter>(client, htmlRenderer, "motivationsFilter");
            await GenerateFilterContent<PayFilter>(client, htmlRenderer, "motivationsFilter");
            await GenerateFilterContent<SchemeLengthFilter>(client, htmlRenderer, "motivationsFilter");
        }

        private static async Task GenerateSchemesContent(ContentfulClient client, HtmlRenderer htmlRenderer)
        {
            var builder = QueryBuilder<Scheme>.New.ContentTypeIs("scheme").Include(1);

            var schemes = await client.GetEntries<Scheme>(builder);

            // ensure we order by size desc, so we don't have to sort at run time
            var schemesBiggestFirst = schemes.OrderByDescending(s => s.Size);

            foreach (Scheme scheme in schemesBiggestFirst)
            {
                Console.WriteLine($"new Scheme(\"{scheme.Name}\",");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortDescription, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortCost, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortBenefits, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.ShortTime, htmlRenderer)},");
                Console.WriteLine($"\"{scheme.Url}\", {scheme.Size},");

                Console.Write("new string[] {");
                if (scheme.PayFilterAspects != null)
                {
                    foreach (var payFilter in scheme.PayFilterAspects)
                    {
                        Console.WriteLine($"\"{Slugify(payFilter.Name)}\",");
                    }
                }

                if (scheme.MotivationsFilterAspects != null)
                {
                    foreach (var motivationsFilter in scheme.MotivationsFilterAspects)
                    {
                        Console.WriteLine($"\"{Slugify(motivationsFilter.Name)}\",");
                    }
                }

                if (scheme.SchemeLengthFilterAspects != null)
                {
                    foreach (var schemeLengthFilter in scheme.SchemeLengthFilterAspects)
                    {
                        Console.WriteLine($"\"{Slugify(schemeLengthFilter.Name)}\",");
                    }
                }

                Console.WriteLine("},");

                Console.WriteLine($"{await AsHtmlString(scheme.DetailsPageOverride, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Description, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Cost, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Responsibility, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.Benefits, htmlRenderer)},");
                Console.WriteLine($"{await AsHtmlString(scheme.CaseStudies, htmlRenderer)},");
                Console.WriteLine($"\"{scheme.OfferHeader}\",");
                Console.WriteLine($"{await AsHtmlString(scheme.Offer, htmlRenderer)}");
                Console.WriteLine("),");
            }
        }

        private static HtmlRenderer CreateHtmlRenderer()
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
            return htmlRenderer;
        }

        private static async Task GenerateFilterContent<T>(ContentfulClient client,
            HtmlRenderer htmlRenderer, string filterTypeName)
        where T : IFilter
        {
            var builder = QueryBuilder<T>.New.ContentTypeIs(filterTypeName);

            var filters = await client.GetEntries<T>(builder);

            //todo: add order to content
            //var orderedFilters = filters.OrderBy(s => s.Order);

            foreach (T filter in filters)
            {
                Console.WriteLine($"new {typeof(T).Name}(\"{Slugify(filter.Name)}\",");
                Console.WriteLine($"\"{filter.Description}\",");
                //Console.WriteLine($"\"{filter.Order}\"");
                Console.WriteLine("),");
            }
        }

        private static string Slugify(string name)
        {
            return name.ToLower().Replace(' ', '-');
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
