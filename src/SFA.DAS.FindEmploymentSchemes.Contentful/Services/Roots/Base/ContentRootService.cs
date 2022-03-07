﻿using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base
{
    public class ContentRootService
    {
        private readonly HtmlRenderer _htmlRenderer;

        public ContentRootService(HtmlRenderer htmlRenderer)
        {
            _htmlRenderer = htmlRenderer;
        }

        //this one might not belong here
        protected static string ToFilterAspectId(Model.Api.IFilter filter, string filterPrefix)
        {
            return $"{filterPrefix}--{Slugify(filter.Name)}";
        }

        protected static string Slugify(string? name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return name.ToLower().Replace(' ', '-');
        }

        protected async Task<HtmlString?> ToHtmlString(Document? document)
        {
            if (document == null)
                return null;

            string html = await _htmlRenderer.ToHtml(document);

            return ToNormalisedHtmlString(html);
        }

        /// <remarks>
        /// Should be private, but Contentful's .net library is not very test friendly (HtmlRenderer.ToHtml can't be mocked).
        /// We'd have to introduce a level of indirection to test this, were it private.
        /// </remarks>
        public static HtmlString ToNormalisedHtmlString(string html)
        {
            // replace left/right quotation marks with regular old quotation marks
            html = html.Replace('“', '"').Replace('”', '"');

            // sometimes contentful uses a \r and sometimes a \r\n - nice!
            // we could strip these out instead
            html = html.Replace("\r\n", "\r");
            html = html.Replace("\r", "\r\n");

            return new HtmlString(html);
        }

        protected IEnumerable<T> FilterValidUrl<T>(IEnumerable<T> roots, ILogger logger)
            where T : IRootContent
        {
            var rootsWithValidUrl = roots.Where(p => !string.IsNullOrWhiteSpace(p.Url));

            int numberExcluded = roots.Count() - rootsWithValidUrl.Count();
            if (numberExcluded > 0)
            {
                logger.LogWarning("Had to exclude {NumberExcluded} root content items for blank urls", numberExcluded);
            }

            return rootsWithValidUrl;
        }
    }
}
