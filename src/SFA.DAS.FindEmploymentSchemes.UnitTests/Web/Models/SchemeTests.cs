using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Models
{
    public class SchemeTests
    {
        [Theory]
        [InlineData("az", "az")]
        [InlineData("AZ", "AZ")]
        [InlineData("a10", "a10")]
        [InlineData("a-", "a-")]
        [InlineData("a_", "a_")]
        [InlineData("a:", "a:")]
        [InlineData("a.", "a.")]
        //[InlineData("a", "a!\"£$%^&*()+=")]
        //[InlineData("a-_:.", "-_:.!\"£$%^&*()+=a-_:.")]
        public void Constructor_HtmlId(string stringExpectedHtmlId, string url)
        {
            var scheme = TestScheme(url);

            Assert.Equal(stringExpectedHtmlId, scheme.HtmlId);
        }

        private static Scheme TestScheme(string url)
        {
            return new Scheme("Apprenticeships",
                "",
                "Find out more about Apprenticeships",
                new HtmlString(
                    @"<p>Paid employment for over 16s, combining work and study in a job allowing you to develop your workforce and business.</p>"),
                "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading",
                new HtmlString(
                    @"<p>Apprentice minimum wage and 5% training contribution depending on business size</p>"),
                new HtmlString(@"<p>You develop a motivated, skilled, and qualified workforce</p>"),
                new HtmlString(@"<p>Minimum of 12 months employment</p>"),
                "Both",
                "16 and over",
                "Free or costed",
                "Over 1 year",
                url, 403000,
                new System.Collections.Generic.List<SchemeFilterAspect>(),
                new string[]
                {
                    "pay--minimum-wage", "motivations--full-time-role", "motivations--diversity-or-responsibility",
                    "scheme-length--4-months-to-12-months", "scheme-length--a-year-or-more",
                },
                null,
                new HtmlString(
                    @"<p>Preamble goes here</p>"),
                new HtmlString(
                    @"<p>Applies to England</p><p>Apprenticeships are for those aged 16 or over and combine working with studying to gain skills and knowledge in a job role.</p><p>Apprentices can be new or current employees.</p><p>Your apprentice must:</p><ul class =""govuk-list govuk-list--bullet""><li>work with experienced staff</li><li>learn skills relevant to your organisation</li><li>get time for training or study during their working week (at least 20% of their normal working hours)</li></ul><p></p>")
               
            );
        }
    }
}