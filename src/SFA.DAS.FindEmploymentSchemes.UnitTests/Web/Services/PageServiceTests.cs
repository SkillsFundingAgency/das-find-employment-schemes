using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class PageServiceTests
    {
        public const string CookiePageUrl = "Cookies";

        public Fixture Fixture { get; set; }
        public IEnumerable<Page> Pages { get; set; }
        public IContentService ContentService { get; set; }
        public IContent Content { get; set; }
        public PageService PageService { get; set; }

        public PageServiceTests()
        {
            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(global::Contentful.Core.Models.IContent),
                    typeof(Paragraph)));

            Content = A.Fake<IContent>();

            ContentService = A.Fake<IContentService>();

            Pages = Fixture.CreateMany<Page>(3);

            A.CallTo(() => Content.Pages)
                .Returns(Pages);

            PageService = new PageService(ContentService);
        }

        //[Fact]
        //public void GetPageModel_CookiePageUrlReturnsCookiesViewNameTest()
        //{
        //    //var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));
        //    //var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

        //    //var pages = new[] { analyticsPage }
        //    //    .Concat(Pages)
        //    //    .Concat(new[] { marketingPage }).ToArray();

        //    //A.CallTo(() => Content.Pages)
        //    //    .Returns(pages);

        //    var model = PageService.GetPageModel(CookiePageUrl);

        //    Assert.Equal("Cookies", model.ViewName);
        //}

        //[Fact]
        //public void Page_CookiePageUrlReturnsCookiePageModelTest()
        //{
        //    var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));
        //    var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

        //    var pages = new[] { analyticsPage }
        //        .Concat(Pages)
        //        .Concat(new[] { marketingPage }).ToArray();

        //    A.CallTo(() => Content.Pages)
        //        .Returns(pages);

        //    var (viewName, page) = PageService.Page(CookiePageUrl, Content);

        //    Assert.IsType<CookiePage>(page);
        //    var cookiePage = (CookiePage)page;
        //    Assert.Equal(analyticsPage, cookiePage.AnalyticsPage);
        //    Assert.Equal(marketingPage, cookiePage.MarketingPage);
        //}

        //[Fact]
        //public void Page_CookiePageUrlWithMissingAnalyticsPageThrowsExceptionTest()
        //{
        //    var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));

        //    var pages = new[] { analyticsPage }
        //        .Concat(Pages).ToArray();

        //    A.CallTo(() => Content.Pages)
        //        .Returns(pages);

        //    Assert.ThrowsAny<Exception>(() => PageService.Page(CookiePageUrl, Content));
        //}

        //[Fact]
        //public void Page_CookiePageUrlWithMissingMarketingPageThrowsExceptionTest()
        //{
        //    var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

        //    var pages = new[] { marketingPage }
        //        .Concat(Pages).ToArray();

        //    A.CallTo(() => Content.Pages)
        //        .Returns(pages);

        //    Assert.ThrowsAny<Exception>(() => PageService.Page(CookiePageUrl, Content));
        //}

        [Fact]
        public void GetPageModel_UnknownPageUrlReturnsNullModelTest()
        {
            string unknowUrl = nameof(unknowUrl);

            var model = PageService.GetPageModel(unknowUrl);

            Assert.Null(model);
        }

        [Fact]
        public void GetPageModel_ErrorCheckPageUrlThrowsExceptionTest()
        {
            string errorCheckUrl = "error-check";

            Assert.Throws<NotImplementedException>(() => PageService.GetPageModel(errorCheckUrl));
        }

        [Fact]
        public void RedirectPreview_NonRedirectUrlReturnsNullRouteNameTest()
        {
            const string nonRedirectUrl = "dont-redirect";

            var (routeName, _) = PageService.RedirectPreview(nonRedirectUrl);

            Assert.Null(routeName);
        }

        [Fact]
        public async Task GetPageModelPreview_IsPreviewIsTrueTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            // act
            var model = await PageService.GetPageModelPreview(Pages.First().Url);

            Assert.True(model.Preview.IsPreview);
        }

        [Fact]
        public async Task GetPageModelPreview_ContentNull_PreviewErrorTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            var page = new Page("title", "url", null);

            var pages = new[] {page};

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            // act
            var model = await PageService.GetPageModelPreview(page.Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Content must not be blank", e.Value));
        }

        [Fact]
        public async Task GetPageModelPreview_TitleNull_PreviewErrorTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            var page = new Page(null, "url", new HtmlString("content"));

            var pages = new[] { page };

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            // act
            var model = await PageService.GetPageModelPreview(page.Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Title must not be blank", e.Value));
        }
    }
}
