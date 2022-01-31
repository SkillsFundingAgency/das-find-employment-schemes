using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using Xunit;
using Document = Contentful.Core.Models.Document;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentServiceTests
    {
        public Fixture Fixture { get; }
        public IContentfulClient ContentfulClient { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }
        public ILogger<ContentService> Logger { get; set; }
        public ContentfulCollection<Page> PagesCollection { get; set; }
        public ContentfulCollection<Scheme> SchemesCollection { get; set; }
        public ContentfulCollection<Filter> FiltersCollection { get; set; }
        public ContentService ContentService { get; set; }
        public CompareLogic CompareLogic { get; set; }

        public ContentServiceTests()
        {
            Fixture = new Fixture();

            ContentfulClient = A.Fake<IContentfulClient>();
            HtmlRenderer = A.Fake<HtmlRenderer>();
            Logger = A.Fake<ILogger<ContentService>>();

            PagesCollection = new ContentfulCollection<Page> { Items = Array.Empty<Page>() };
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(PagesCollection);

            SchemesCollection = new ContentfulCollection<Scheme> { Items = Array.Empty<Scheme>() };
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Scheme>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(SchemesCollection);

            FiltersCollection = new ContentfulCollection<Filter> { Items = Array.Empty<Filter>() };
            //A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
            //    .Returns(payFiltersCollection).Once()
            //    .Then
            //    .Returns(new ContentfulCollection<Filter>());

            // decouples us from the order of fetching filters
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(FiltersCollection);

            ContentService = new ContentService(ContentfulClient, HtmlRenderer, Logger);

            CompareLogic = new CompareLogic();
        }

        [Theory]
        [InlineData("pay--the-name", "the name")]
        [InlineData("pay--thename", "thename")]
        [InlineData("pay--the-name", "the-name")]
        //todo: stop double spaces, so code doesn't get confused with prefix/name separator?
        [InlineData("pay--the--name", "the  name")]
        [InlineData("pay--", "")]
        [InlineData("pay--1234567890-qwertyuiop-asdfghjkl-zxcvbnm", "1234567890 qwertyuiop asdfghjkl zxcvbnm")]
        public async Task Update_FilterIdTests(string expectedFilterAspectId, string filterName)
        {
            var filters = Fixture.CreateMany<Filter>(1).ToList();
            filters.First().Name = filterName;
            FiltersCollection.Items = filters;

            var content = await ContentService.Update();

            Assert.Equal(expectedFilterAspectId, content.PayFilter.Aspects.First().Id);
        }

        // Contentful's .net library is not very test friendly: HtmlRenderer.ToHtml can't be mocked
        // we'd have to introduce a level of indirection to test this
        // or we _could_ make ToHtmlString in ContentService public and test it directly
        //[Theory]
        //[InlineData("\"", "“")]
        //public async Task Update_HtmlQuirksTest(string expectedHtmlStringValue, string unescapedHtml)
        //{
        //    Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        //        .ForEach(b => Fixture.Behaviors.Remove(b));
        //    Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        //    Fixture.Customizations.Add(
        //        new TypeRelay(
        //            typeof(IContent),
        //            typeof(Paragraph)));

        //    var pages = Fixture.CreateMany<Page>(1).ToList();
        //    PagesCollection.Items = pages;

        //    A.CallTo(() => HtmlRenderer.ToHtml(A<Document>.Ignored))
        //        .Returns(unescapedHtml);

        //    var content = await ContentService.Update();

        //    Assert.Equal(expectedHtmlStringValue, content.Pages.First().Content.Value);
        //}

        [Theory]
        [InlineData("\"", "“")]
        [InlineData("\"", "”")]
        [InlineData("\"\"", "“”")]
        [InlineData("\r\n", "\r")]
        [InlineData("\r\n", "\r\n")]
        [InlineData("\r\n\r\n", "\r\r\n")]
        [InlineData("\r\nn", "\rn")]
        [InlineData("<br>", "<br>")]
        public async Task ToNormalisedHtmlString_Tests(string expectedHtmlStringValue, string html)
        {
            var result = ContentService.ToNormalisedHtmlString(html);

            Assert.Equal(expectedHtmlStringValue, result.Value);
        }

        [Fact]
        public void Content_IsGeneratedContentBeforeUpdate()
        {
            // xunit v3 supports this, but the vs & msbuild test runners aren't ready yet
            //Assert.Equivalent

            // so use the old faithful...

            var compareResult = CompareLogic.Compare(new GeneratedContent(), ContentService.Content);

            Assert.True(compareResult.AreEqual);
        }

        [Fact]
        public async Task Content_IsNotGeneratedContentAfterUpdate()
        {
            await ContentService.Update();

            var compareResult = CompareLogic.Compare(new GeneratedContent(), ContentService.Content);

            Assert.False(compareResult.AreEqual);
        }
    }
}
