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

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{

    public class ContactServiceTests : RootServiceTestBase<Contact, ContactService>
    {

        public HtmlRenderer htmlRenderer { get; set; }

        public ContactServiceTests()
        {

            htmlRenderer = A.Fake<HtmlRenderer>();

        }

        [Fact(DisplayName = "ContactService - GetLandingPage - WithMatchingPage - ReturnsLandingPage")]
        public async Task InterimService_GetLandingPage_WithMatchingPage_ReturnsLandingPage()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactService = new ContactService(htmlRenderer);

            var entries = new ContentfulCollection<ContactPage>();

            entries.Items = [

                new ContactPage()
                {

                    ContactPageTitle = "ContactPageTitle",

                    Contacts = new List<Contact>()
                    {

                        new Contact() { SectionName = "SectionName" }

                    }

                }

            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Returns(entries);

            var result = await contactService.GetContactPage(contentfulClient);

            Assert.NotNull(result);

            Assert.Equal("ContactPageTitle", result.ContactPageTitle);

        }

        [Fact(DisplayName = "ContactService - GetContactPage - WithNoMatchingPage - ReturnsNull")]
        public async Task InterimService_GetContactPage_WithNoMatchingPage_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactService = new ContactService(htmlRenderer);

            var fakeResult = new ContentfulCollection<ContactPage> { Items = new List<ContactPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await contactService.GetContactPage(contentfulClient);

            Assert.Null(result);

        }

        [Fact(DisplayName = "ContactService - GetContactPage - WithException - ReturnsNull")]
        public async Task InterimService_GetContactPage_WithException_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactService = new ContactService(htmlRenderer);

            var fakeResult = new ContentfulCollection<ContactPage> { Items = new List<ContactPage>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<ContactPage>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await contactService.GetContactPage(contentfulClient);

            Assert.Null(result);

        }


    }

}
