using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
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
                new HtmlString(
                    @"<p>Paid employment for over 16s, combining work and study in a job allowing you to develop your workforce and business.</p>"),
                new HtmlString(
                    @"<p>Apprentice minimum wage and 5% training contribution depending on business size</p>"),
                new HtmlString(@"<p>You develop a motivated, skilled, and qualified workforce</p>"),
                new HtmlString(@"<p>Minimum of 12 months employment</p>"),
                url, 403000,
                new string[]
                {
                    "pay--minimum-wage", "motivations--full-time-role", "motivations--diversity-or-responsibility",
                    "scheme-length--4-months-to-12-months", "scheme-length--a-year-or-more",
                },
                new CaseStudy[]
                {
                    new CaseStudy("name", "title", new HtmlString(@"<p>Case study content here</p>"))
                },
                new HtmlString(
                    @"<p>Preamble goes here</p>"),
                new HtmlString(
                    @"<p>Applies to England</p><p>Apprenticeships are for those aged 16 or over and combine working with studying to gain skills and knowledge in a job role.</p><p>Apprentices can be new or current employees.</p><p>Your apprentice must:</p><ul class =""govuk-list govuk-list--bullet""><li>work with experienced staff</li><li>learn skills relevant to your organisation</li><li>get time for training or study during their working week (at least 20% of their normal working hours)</li></ul><p></p>"),
                new HtmlString(
                    @"<p>You must pay apprentices <a href=""https://www.gov.uk/national-minimum-wage-rates"" title="""" class=""govuk-link"">the minimum wage</a>.</p><p>You may have to contribute 5% of the apprentice's training and assessment costs - depending on the size of your business and the age of your apprentice.</p>"),
                new HtmlString(
                    @"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li>a safe work environment</li><li>an induction which includes explaining relevant policies and procedures</li><li>opportunities for the student to develop their technical skills within your industry</li><li>20% 'Off-the-job' training delivered by a training provider during your apprentice&#39;s normal working hours</li><li>'On-the-job' training and additional support to ensure your apprentice's success</li></ul><p>Employers must also have:</p><ul class =""govuk-list govuk-list--bullet""><li>a signed apprenticeship agreement and apprenticeship commitment statement from the learner and training provider</li></ul><p></p>"),
                new HtmlString(
                    @"<p>Find out <a href=""https://www.apprenticeships.gov.uk/"" title="""" class=""govuk-link"">how apprenticeships can benefit your business</a>.</p>"),
                new HtmlString(
                    @"<p>Read about <a href=""https://www.apprenticeships.gov.uk/employers/real-stories-employers"" title="""" class=""govuk-link"">how businesses are using incentive payments to hire apprentices</a>.</p>"),
                "Offer an apprenticeship",
                new HtmlString(
                    @"<p>Get started with offering an apprenticeship.</p><p><a href=""https://www.apprenticeships.gov.uk/employers/hiring-an-apprentice"" title="""" class=""govuk-link"">Hire an apprentice</a></p><p>Alternatively, call <a href=""tel:08000150400"" title="""" class=""govuk-link"">08000 150 400</a></p>")
            );
        }
    }
}