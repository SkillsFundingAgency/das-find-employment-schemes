using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Enums;
using SFA.DAS.FindEmploymentSchemes.Web.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.References;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public static class InterimPageService
    {

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private static ILogger _logger;

        private static IViewRenderService _viewRenderService;

        private static HtmlRenderer _htmlRenderer;

        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static void Initialize(ILogger logger, IViewRenderService viewRenderService, HtmlRenderer htmlRenderer)
        {

            _logger = logger;

            _viewRenderService = viewRenderService;

            _htmlRenderer = htmlRenderer;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static TagBuilder GenerateView(Scheme scheme)
        {

            return GenerateInterimPageAsync(scheme);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        private static TagBuilder GenerateInterimPageAsync(Scheme scheme)
        {

            return GenerateComponents(scheme);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        private static TagBuilder GenerateComponents(Scheme scheme)
        {

            var pageLayoutRow = new TagBuilder("div");

            pageLayoutRow.AddCssClass($"{GovukReferences.GOVUK_GRID_ROW} {GovukReferences.PADDING_RIGHT_5}");

            foreach (InterimPageComponent component in scheme.Components)
            {

                GenerateComponent(component, pageLayoutRow);

            }

            return pageLayoutRow;

        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, string> MappedTypesToPartials = new Dictionary<string, string>()
        {

            { InterimPageReferences.InterimContainerType, InterimPageReferences._InterimContainerPartial },

            { InterimPageReferences.InterimContentSectionsType, InterimPageReferences._ContentsSectionPartial },

            { InterimPageReferences.InterimVideoSectionType, InterimPageReferences._InterimVideoPartial },

            { InterimPageReferences.InterimCaseStudiesType, InterimPageReferences._InterimCaseStudiesPartial },

            { InterimPageReferences.InterimAccordionType, InterimPageReferences._InterimAccordionPartial }

        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="tag"></param>
        private static void GenerateComponent(InterimPageComponent component, TagBuilder tag)
        {

            try
            {

                var result = _viewRenderService.RenderToStringAsync(

                    MappedTypesToPartials[component.ComponentType ?? string.Empty],

                    component

                ).Result;

                tag.InnerHtml.AppendHtml(result);

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to generate componeont for type {ComponentType}.", component.ComponentType);

            }

        }

        public static string GenerateSubComponent(InterimPageComponent component)
        {

            try
            {

                var result = _viewRenderService.RenderToStringAsync(

                    MappedTypesToPartials[component.ComponentType ?? string.Empty],

                    component

                ).Result;

                return result;

            }
            catch (Exception _exception)
            {

                _logger.LogError(_exception, "Unable to generate componeont for type {ComponentType}.", component.ComponentType);

                return string.Empty;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static HtmlString? ToHtmlString(Document? document)
        {
            if (document == null)
            {

                return null;

            }
                
            string html = _htmlRenderer.ToHtml(document).Result;

            return ToNormalisedHtmlString(html);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlString ToNormalisedHtmlString(string html)
        {

            html = html.Replace('“', '"').Replace('”', '"');

            html = html.Replace("\r\n", "\r");

            html = html.Replace("\r", "\r\n");

            return new HtmlString(html);

        }

    }

}
