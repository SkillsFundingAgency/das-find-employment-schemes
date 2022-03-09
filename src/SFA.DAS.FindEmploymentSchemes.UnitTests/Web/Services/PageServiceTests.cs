using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Xunit;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class PageServiceTests
    {
        public const string CookiePageUrl = "Cookies";

        public Fixture Fixture { get; set; }
        public IEnumerable<Page> Pages { get; set; }
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

            Pages = Fixture.CreateMany<Page>(3);

            A.CallTo(() => Content.Pages)
                .Returns(Pages);

            PageService = new PageService();
        }

        [Fact]
        public void Page_StandardPageUrlReturnsDefaultViewNameTest()
        {
            string pageUrl = Pages.Skip(1).First().Url;

            var (viewName, page) = PageService.Page(pageUrl, Content);

            Assert.Null(viewName);
        }

        [Fact]
        public void Page_StandardPageUrlReturnsPageTest()
        {
            Page expectedPage = Pages.Skip(1).First();

            var (viewName, page) = PageService.Page(expectedPage.Url, Content);

            Assert.Equal(expectedPage, page);
        }

        [Fact]
        public void Page_CookiePageUrlReturnsCookiesViewNameTest()
        {
            var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));
            var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

            var pages = new[] { analyticsPage }
                .Concat(Pages)
                .Concat(new [] { marketingPage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            var (viewName, page) = PageService.Page(CookiePageUrl, Content);

            Assert.Equal("Cookies", viewName);
        }

        [Fact]
        public void Page_CookiePageUrlReturnsCookiePageModelTest()
        {
            var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));
            var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

            var pages = new[] { analyticsPage }
                .Concat(Pages)
                .Concat(new[] { marketingPage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            var (viewName, page) = PageService.Page(CookiePageUrl, Content);

            Assert.IsType<CookiePage>(page);
            var cookiePage = (CookiePage) page;
            Assert.Equal(analyticsPage, cookiePage.AnalyticsPage);
            Assert.Equal(marketingPage, cookiePage.MarketingPage);
        }

        [Fact]
        public void Page_CookiePageUrlWithMissingAnalyticsPageThrowsExceptionTest()
        {
            var analyticsPage = new Page("", "analyticscookies", new HtmlString(""));

            var pages = new[] { analyticsPage }
                .Concat(Pages).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            Assert.ThrowsAny<Exception>(() => PageService.Page(CookiePageUrl, Content));
        }

        [Fact]
        public void Page_CookiePageUrlWithMissingMarketingPageThrowsExceptionTest()
        {
            var marketingPage = new Page("", "marketingcookies", new HtmlString(""));

            var pages = new[] { marketingPage }
                .Concat(Pages).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            Assert.ThrowsAny<Exception>(() => PageService.Page(CookiePageUrl, Content));
        }

        [Fact]
        public void Page_UnknownPageUrlReturnsNullPageTest()
        {
            string unknowUrl = nameof(unknowUrl);

            var (viewName, page) = PageService.Page(unknowUrl, Content);

            Assert.Null(page);
        }

        [Fact]
        public void Page_ErrorCheckPageUrlThrowsExceptionTest()
        {
            string errorCheckUrl = "error-check";

            Assert.Throws<NotImplementedException>(() => PageService.Page(errorCheckUrl, Content));
        }

        [Fact]
        public void RedirectPreview_NonRedirectUrlReturnsNullRouteNameTest()
        {
            const string nonRedirectUrl = "dont-redirect";

            var (routeName, _) = PageService.RedirectPreview(nonRedirectUrl);

            Assert.Null(routeName);
        }
    }
}
