﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var httpClient = new HttpClient();
            var client = new ContentfulClient(httpClient,
                new ContentfulOptions
                {
                    SpaceId = config["SpaceId"],
                    DeliveryApiKey = config["DeliveryApiKey"],
                    Environment = config["Environment"]
                });

            var contenfulClientFactory = new ContentfulClientFactory(new[] {client});

            var htmlRenderer = ContentService.CreateHtmlRenderer();

            var contentService = new ContentService(contenfulClientFactory, htmlRenderer, new NullLogger<ContentService>());

            var content = await contentService.Update();

            Console.Write(Preamble());

            GenerateGeneratedContentWarning();

            GenerateSchemesContent(content.Schemes);

            GenerateFilterContent(content.MotivationsFilter);
            GenerateFilterContent(content.PayFilter);
            GenerateFilterContent(content.SchemeLengthFilter);

            GeneratePagesContent(content.Pages);
            GenerateCaseStudyPagesContent(content.CaseStudyPages);

            Console.WriteLine(Closing());
        }

        private static void GenerateGeneratedContentWarning()
        {
            Console.WriteLine(@"        //  _    _                                _
        // | |  | |                              | |
        // | |__| | ___ _   _   _   _  ___  _   _| |
        // |  __  |/ _ \ | | | | | | |/ _ \| | | | |
        // | |  | |  __/ |_| | | |_| | (_) | |_| |_|
        // |_|  |_|\___|\__, |  \__, |\___/ \__,_(_)
        //               __/ |   __/ |
        //               |___/   |___/               
        //
        // this file is generated by the ContentCodeGenerator utility, so don't directly make changes here!!!
        //
");
        }

        private static void GeneratePagesContent(IEnumerable<Page> pages)
        {
            string typeName = GenerateProperty<Page>();

            foreach (var page in pages)
            {
                Console.WriteLine($"            new {typeName}(\"{page.Title}\",");
                Console.WriteLine($"                \"{page.Url}\",");
                Console.WriteLine($"                {GenerateHtmlString(page.Content)}");
                Console.WriteLine("            ),");
            }

            Console.WriteLine(@"        };");
        }

        private static void GenerateCaseStudyPagesContent(IEnumerable<CaseStudyPage> caseStudyPages)
        {
            string typeName = GenerateProperty<CaseStudyPage>();

            foreach (var caseStudyPage in caseStudyPages)
            {
                Console.WriteLine($"            new {typeName}(\"{caseStudyPage.Title}\",");
                Console.WriteLine($"                \"{caseStudyPage.Url}\",");
                Console.WriteLine($"                Schemes.First(x => x.Name == \"{caseStudyPage.Scheme.Name}\"),");
                Console.WriteLine($"                {GenerateHtmlString(caseStudyPage.Content)}");
                Console.WriteLine("            ),");
            }

            Console.WriteLine(@"        };");
        }

        private static void GenerateSchemesContent(IEnumerable<Scheme> schemes)
        {
            string typeName = GenerateProperty<Scheme>();

            foreach (var scheme in schemes)
            {
                Console.WriteLine($"            new {typeName}(\"{scheme.Name}\",");
                Console.WriteLine($"                {GenerateHtmlString(scheme.ShortDescription)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.ShortCost)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.ShortBenefits)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.ShortTime)},");
                Console.WriteLine($"                \"{scheme.Url}\", {scheme.Size},");

                Console.Write("             new string[] {");
                Console.Write($"                {string.Join(", ", scheme.FilterAspects.Select(f => $"\"{f}\""))}");
                Console.WriteLine("             },");

                //todo: will have to support existing case study content until new content is available for release
                GenerateCaseStudies(scheme.CaseStudies);

                Console.WriteLine($"                {GenerateHtmlString(scheme.CaseStudiesPreamble)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.DetailsPageOverride)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.Description)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.Cost)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.Responsibility)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.Benefits)},");
                Console.WriteLine($"                \"{scheme.OfferHeader}\",");
                Console.WriteLine($"                {GenerateHtmlString(scheme.Offer)},");
                Console.WriteLine($"                {GenerateHtmlString(scheme.AdditionalFooter)},");

                GenerateSubSchemesContent(scheme.SubSchemes);


                Console.WriteLine("                ),");
            }

            Console.WriteLine(@"        };");
        }

        private static void GenerateSubSchemesContent(IEnumerable<SubScheme> subSchemes)
        {
            string typeName = nameof(SubScheme);

            Console.WriteLine($"                new {typeName}[] {{");
            
            foreach (var subScheme in subSchemes)
            {
                Console.WriteLine($"                    new {typeName}(\"{subScheme.Title}\",");
                Console.WriteLine($"                    {GenerateHtmlString(subScheme.Summary)},");
                Console.WriteLine($"                    {GenerateHtmlString(subScheme.Content)}");
                Console.WriteLine("                    ),");
            }

            Console.WriteLine(@"                }");
        }

        private static void GenerateFilterContent(Filter filter)
        {
            string upperName = $"{char.ToUpperInvariant(filter.Name[0])}{filter.Name.Substring(1)}";

            Console.WriteLine($@"       private Filter? _{filter.Name}Filter;
        public Filter {upperName}Filter => _{filter.Name}Filter ??= new Filter(""{filter.Name}"", ""{filter.Description}"", new FilterAspect[]
        {{");

            foreach (var filterAspect in filter.Aspects)
            {
                Console.WriteLine($"            new FilterAspect(\"{filterAspect.Id}\", \"{filterAspect.Description}\"),");
            }

            Console.WriteLine("        });");
        }

        private static void GenerateCaseStudies(IEnumerable<CaseStudy> caseStudies)
        {
            string typeName = nameof(CaseStudy);

            Console.WriteLine($"                new {typeName}[] {{");

            foreach (var caseStudy in caseStudies)
            {
                Console.WriteLine($"                    new {typeName}(\"{caseStudy.Name}\",");
                Console.WriteLine($"                    \"{caseStudy.DisplayTitle}\",");
                Console.WriteLine($"                    {GenerateHtmlString(caseStudy.Content)}");
                Console.WriteLine("                    ),");
            }

            Console.WriteLine(@"                },");
        }

        private static string Preamble()
        {
            return @"using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Content
{
    public class GeneratedContent : IContent
    {
";
        }

        private static string Closing()
        {
            return @"    }
}";
        }

        //todo: rename EnumerableProperty?
        private static string GenerateProperty<T>()
        {
            string typeName = typeof(T).Name;
            string backingName = $"_{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1)}s";

            Console.WriteLine($@"        private IEnumerable<{typeName}>? {backingName};
        public IEnumerable<{typeName}> {typeName}s => {backingName} ??= new {typeName}[]
        {{");

            return typeName;
        }

        private static string GenerateHtmlString(HtmlString? content)
        {
            if (content == null)
                return "null";

            return $"new HtmlString(@\"{content.Value.Replace("\"", "\"\"")}\")";
        }
    }
}
