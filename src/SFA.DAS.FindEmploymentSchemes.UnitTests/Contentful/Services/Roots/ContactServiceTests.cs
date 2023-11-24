using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{

    public class ContactServiceTests : RootServiceTestBase<Contact, ContactService>
    {

        public IEnumerable<Contact> Contacts { get; set; }

        public IContactService CaseStudyPageService { get; set; }

        public ContactServiceTests()
        {

            CaseStudyPageService = new ContactService(HtmlRenderer, Logger);

        }

        [Fact(DisplayName = "ContactServiceTests - GetContacts - SameNumberOfContactsTest")]
        public async Task ContactServiceTests_GetContacts_SameNumberOfContactsTest()
        {

            const int numberOfContacts = 5;

            Contacts = Fixture.CreateMany<Contact>(1);

            Fixture.Inject(Fixture.Create<Contact>());

            ContentfulCollection.Items = Fixture.CreateMany<Contact>(numberOfContacts);

            IEnumerable<Contact> contacts = await CaseStudyPageService.GetContacts(ContentfulClient);

            Assert.NotNull(contacts);

            Assert.Equal(numberOfContacts, contacts.Count());

        }

    }

}
