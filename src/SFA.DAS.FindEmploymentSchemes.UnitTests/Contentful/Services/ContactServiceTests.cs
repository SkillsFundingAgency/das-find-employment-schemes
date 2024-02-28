using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{

    public class ContactServiceTests
    {

        public HtmlRenderer htmlRenderer { get; set; }

        public ContactServiceTests()
        {

            htmlRenderer = A.Fake<HtmlRenderer>();

        }

        [Fact(DisplayName = "GetContactPage - WithMatchingPage - ReturnsContactPage")]
        public async Task GetContactPage_WithMatchingPage_ReturnsContactPage()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new ContactService(htmlRenderer);

            var entries = new ContentfulCollection<ContactPage>();

            entries.Items = [new ContactPage { ContactPageTitle = "Test Contact Page" }];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Returns(entries);

            var result = await contactPageService.GetContactPage(contentfulClient);

            Assert.NotNull(result);

            Assert.Equal("Test Contact Page", result.ContactPageTitle);

        }

        [Fact(DisplayName = "GetContactPage - WithNoMatchingPage - ReturnsNull")]
        public async Task GetContactPage_WithNoMatchingPage_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new ContactService(htmlRenderer);

            var fakeResult = new ContentfulCollection<ContactPage> { Items = new List<ContactPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await contactPageService.GetContactPage(contentfulClient);

            Assert.Null(result);

        }

        [Fact(DisplayName = "GetContactPage - WithException - ReturnsNull")]
        public async Task GetContactPage_WithException_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new ContactService(htmlRenderer);

            var fakeResult = new ContentfulCollection<ContactPage> { Items = new List<ContactPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await contactPageService.GetContactPage(contentfulClient);

            Assert.Null(result);

        }

    }

}
