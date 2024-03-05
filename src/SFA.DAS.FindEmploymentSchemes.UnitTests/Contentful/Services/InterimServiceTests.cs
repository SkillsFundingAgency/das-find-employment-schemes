using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{

    public class InterimServiceTests
    {

        public HtmlRenderer htmlRenderer { get; set; }

        public ILogger<InterimService> Logger { get; set; }

        public InterimServiceTests()
        {

            htmlRenderer = A.Fake<HtmlRenderer>();

            Logger = A.Fake<ILogger<InterimService>>();

        }

        #region Get Landing Page Tests

        [Fact(DisplayName = "InterimService - GetLandingPage - WithMatchingPage - ReturnsLandingPage")]
        public async Task InterimService_GetLandingPage_WithMatchingPage_ReturnsLandingPage()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var entries = new ContentfulCollection<InterimLandingPage>();

            entries.Items = [
                
                new InterimLandingPage() { 
                
                    InterimLandingPageID = "interim-landing-page", 
                
                    InterimLandingPagePreamble = new InterimPreamble(),
                
                    InterimLandingPageTitle = "Test Landing Page" 
                
                }
            
            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimLandingPage>>._, A<CancellationToken>._)).Returns(entries);

            var result = await interimService.GetLandingPage(contentfulClient);

            Assert.NotNull(result);

            Assert.Equal("Test Landing Page", result.InterimLandingPageTitle);

        }

        [Fact(DisplayName = "InterimService - GetLandingPage - WithNoMatchingPage - ReturnsNull")]
        public async Task InterimService_GetLandingPage_WithNoMatchingPage_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimLandingPage> { Items = new List<InterimLandingPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimLandingPage>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await interimService.GetLandingPage(contentfulClient);

            Assert.Null(result);

        }

        [Fact(DisplayName = "InterimService - GetLandingPage - WithException - ReturnsNull")]
        public async Task InterimService_GetLandingPage_WithException_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimLandingPage> { Items = new List<InterimLandingPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimLandingPage>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await interimService.GetLandingPage(contentfulClient);

            Assert.Null(result);

        }

        #endregion

        #region Get Menu Items Tests

        [Fact(DisplayName = "InterimService - GetMenuItems - WithMatchingMenuItems - ReturnsMenuItems")]
        public async Task InterimService_GetMenuItems_WithMatchingMenuItems_ReturnsMenuItems()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var entries = new ContentfulCollection<InterimMenuItem>();

            entries.Items = [

                new InterimMenuItem()
                {

                    InterimMenuItemOrder = 1,

                    InterimMenuItemTitle = "Menu Item Title",

                    InterimMenuItemText = "Menu Item Text",

                    InterimMenuItemSource = "Menu Item Source"

                }

            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimMenuItem>>._, A<CancellationToken>._)).Returns(entries);

            var result = await interimService.GetMenuItems(contentfulClient);

            Assert.NotNull(result);

            Assert.Single(result);

            Assert.Equal("Menu Item Title", result.First().InterimMenuItemTitle);

            Assert.Equal("Menu Item Text", result.First().InterimMenuItemText);

            Assert.Equal("Menu Item Source", result.First().InterimMenuItemSource);

        }

        [Fact(DisplayName = "InterimService - GetMenuItems - WithNoMatchingMenuItems - Returns Empty")]
        public async Task InterimService_GetMenuItems_WithNoMatchingMenuItems_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimMenuItem> { Items = new List<InterimMenuItem>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimMenuItem>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await interimService.GetMenuItems(contentfulClient);

            Assert.Empty(result);

        }

        [Fact(DisplayName = "InterimService - GetMenuItems - WithException - ReturnsEmpty")]
        public async Task InterimService_GetMenuItems_WithException_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimMenuItem> { Items = new List<InterimMenuItem>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimMenuItem>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await interimService.GetMenuItems(contentfulClient);

            Assert.Empty(result);

        }

        #endregion

        #region Get Interim Pages

        [Fact(DisplayName = "InterimService - GetInterimPages - WithMatchingPage - ReturnsContactPage")]
        public async Task InterimService_GetInterimPages_WithMatchingPage_ReturnsContactPage()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var entries = new ContentfulCollection<InterimPage>();

            entries.Items = [

                new InterimPage()
                {

                    InterimPageTitle = "Interim Page Title",

                    InterimPageURL = "Interim Page URL"

                }

            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimPage>>._, A<CancellationToken>._)).Returns(entries);

            var result = await interimService.GetInterimPages(contentfulClient);

            Assert.NotNull(result);

            Assert.Single(result);

            Assert.Equal("Interim Page Title", result.First().InterimPageTitle);

            Assert.Equal("Interim Page URL", result.First().InterimPageURL);

        }

        [Fact(DisplayName = "InterimService - GetInterimPages - WithNoMatchingPage - Returns Empty")]
        public async Task InterimService_GetInterimPages_WithNoMatchingPage_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimPage> { Items = new List<InterimPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimPage>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await interimService.GetInterimPages(contentfulClient);

            Assert.Empty(result);

        }

        [Fact(DisplayName = "InterimService - GetInterimPages - WithException - ReturnsEmpty")]
        public async Task InterimService_GetInterimPages_WithException_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimPage> { Items = new List<InterimPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimPage>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await interimService.GetInterimPages(contentfulClient);

            Assert.Empty(result);

        }

        #endregion

        #region Get Interim Footer

        [Fact(DisplayName = "InterimService - GetFooter - WithMatchingFooter - ReturnsFooterPage")]
        public async Task InterimService_GetFooter_WithMatchingFooter_ReturnsFooterPage()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var entries = new ContentfulCollection<InterimFooterLinks>();

            entries.Items = [

                new InterimFooterLinks()
                {

                    InterimFooterLinksTitle = "InterimFooterLinksTitle",

                    InterimFooterLinksID = "employer-schemes-footer",

                    InterimFooterLinksPrimarySectionTitle = "InterimFooterLinksPrimarySectionTitle",

                    InterimFooterLinksSecondarySectionTitle = "InterimFooterLinksSecondarySectionTitle"

                }

            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimFooterLinks>>._, A<CancellationToken>._)).Returns(entries);

            var result = await interimService.GetFooter(contentfulClient);

            Assert.NotNull(result);

            Assert.Equal("InterimFooterLinksTitle", result.InterimFooterLinksTitle);

            Assert.Equal("employer-schemes-footer", result.InterimFooterLinksID);

            Assert.Equal("InterimFooterLinksPrimarySectionTitle", result.InterimFooterLinksPrimarySectionTitle);

            Assert.Equal("InterimFooterLinksSecondarySectionTitle", result.InterimFooterLinksSecondarySectionTitle);

        }

        [Fact(DisplayName = "InterimService - GetFooter - WithNoMatchingFooter - Returns Empty")]
        public async Task InterimService_GetFooter_WithNoMatchingFooter_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimFooterLinks> { Items = new List<InterimFooterLinks>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimFooterLinks>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await interimService.GetFooter(contentfulClient);

            Assert.Null(result);

        }

        [Fact(DisplayName = "InterimService - GetFooter - WithException - ReturnsEmpty")]
        public async Task InterimService_GetFooter_WithException_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<InterimFooterLinks> { Items = new List<InterimFooterLinks>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<InterimFooterLinks>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await interimService.GetFooter(contentfulClient);

            Assert.Null(result);

        }

        #endregion

        #region Get Beta Banner Tests

        [Fact(DisplayName = "InterimService - GetBetaBanner - WithMatchingBetaBanner - ReturnsBetaBanner")]
        public async Task InterimService_GetBetaBanner_WithMatchingBetaBanner_ReturnsBetaBanner()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var entries = new ContentfulCollection<BetaBanner>();

            entries.Items = [

                new BetaBanner()
                {

                    BetaBannerTitle = "BetaBannerTitle",

                    BetaBannerID = "employer-schemes-beta-banner"

                }

            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<BetaBanner>>._, A<CancellationToken>._)).Returns(entries);

            var result = await interimService.GetBetaBanner(contentfulClient);

            Assert.NotNull(result);

            Assert.Equal("BetaBannerTitle", result.BetaBannerTitle);

            Assert.Equal("employer-schemes-beta-banner", result.BetaBannerID);

        }

        [Fact(DisplayName = "InterimService - GetBetaBanner - WithNoMatchingBetaBanner - Returns Empty")]
        public async Task InterimService_GetBetaBanner_WithNoMatchingBetaBanner_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<BetaBanner> { Items = new List<BetaBanner>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<BetaBanner>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await interimService.GetBetaBanner(contentfulClient);

            Assert.Null(result);

        }

        [Fact(DisplayName = "InterimService - GetBetaBanner - WithException - ReturnsEmpty")]
        public async Task InterimService_GetBetaBanner_WithException_ReturnsEmpty()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var interimService = new InterimService(htmlRenderer, Logger);

            var fakeResult = new ContentfulCollection<BetaBanner> { Items = new List<BetaBanner>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<BetaBanner>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await interimService.GetBetaBanner(contentfulClient);

            Assert.Null(result);

        }

        #endregion

    }

}
