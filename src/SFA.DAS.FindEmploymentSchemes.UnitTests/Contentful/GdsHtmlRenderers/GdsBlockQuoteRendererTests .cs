﻿using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
{
    public class GdsBlockQuoteRendererTests
    {
        [Fact]
        public async Task ToHtml_GdsBlockQuoteRendererTests()
        {
            var renderer = ContentService.CreateHtmlRenderer();
            var doc = new Document
            {
                Content = new List<IContent>
                {
                    new Quote
                    {
                        Content = new List<IContent>
                        {
                            new Paragraph
                            {
                                Content = new List<IContent>
                                {
                                    new Text
                                    {
                                        Value = "\"I was so impressed I agreed to be quoted. I really thought the service was great!\""
                                    }
                                }
                            },
                            new Paragraph
                            {
                                Content = new List<IContent>
                                {
                                    new Text
                                    {
                                        Value = "<b>Bob ServiceUser</b>"
                                    }
                                }
                            },
                            new Paragraph
                            {
                                Content = new List<IContent>
                                {
                                    new Text
                                    {
                                        Value = "Service Users plc"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var html = await renderer.ToHtml(doc);

            Assert.Equal("<div class=\"govuk-inset-text\"><p class=\"govuk-body\">&quot;I was so impressed I agreed to be quoted. I really thought the service was great!&quot;</p><p class=\"govuk-body\">&lt;b&gt;Bob ServiceUser&lt;/b&gt;</p><p class=\"govuk-body\">Service Users plc</p></div>", html);
        }
    }
}