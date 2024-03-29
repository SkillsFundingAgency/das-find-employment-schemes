﻿using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
{
    public class GdsCtaBoxRendererTests
    {
        [Fact]
        public async Task ToHtml_GdsGdsCtaBoxRendererTests()
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
                                        Value = "<cta>This is a"
                                    }
                                }
                            },
                            new Paragraph
                            {
                                Content = new List<IContent>
                                {
                                    new Text
                                    {
                                        Value = "Call To Action"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var html = await renderer.ToHtml(doc);

            Assert.Equal("<section class=\"cx-cta-box\"><p class=\"govuk-body\">This is a</p><p class=\"govuk-body\">Call To Action</p></section>", html);
        }
    }
}
