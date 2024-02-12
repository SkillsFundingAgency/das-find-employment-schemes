using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{

    public class ContactServiceTests : RootServiceTestBase<Contact, ContactService>
    {

        public ContactPage ContactPage { get; set; }

        public IContactService CaseStudyPageService { get; set; }

        public ContactServiceTests()
        {

            CaseStudyPageService = new ContactService(HtmlRenderer);

        }

        //[Fact(DisplayName = "ContactServiceTests - GetContacts - SameNumberOfContactsTest")]
        //public async Task ContactServiceTests_GetContacts_SameNumberOfContactsTest()
        //{

        //    const int numberOfContacts = 5;

        //    ContactPage = Fixture.Create<ContactPage>();

        //    Fixture.Inject(Fixture.Create<Contact>());

        //    ContentfulCollection.Items = Fixture.CreateMany<Contact>(numberOfContacts);

        //    ContactPage _page = await CaseStudyPageService.GetContactPage(ContentfulClient);

        //    Assert.NotNull(_page);

        //}

    }

}
