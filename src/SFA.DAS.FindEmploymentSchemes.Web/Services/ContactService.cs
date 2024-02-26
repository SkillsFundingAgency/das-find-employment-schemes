using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class ContactService : IContactService
    {

        private readonly IContentService _contentService;

        #pragma warning disable CS8618
        public ContactService(IContentService contentService)
        {

            _contentService = contentService;

        }

        public ContactPageModel? GetContactPageModel()
        {

            if(_contentService.Content.ContactPage == null)
            {

                return null;

            }

            return new ContactPageModel()
            {
                
                ContactPageTitle = _contentService.Content.ContactPage.ContactPageTitle,

                InterimBreadcrumbs = _contentService.Content.ContactPage.InterimBreadcrumbs,

                InterimPreamble = _contentService.Content.ContactPage.InterimPreamble,

                Contacts = _contentService.Content.ContactPage.Contacts,

                MenuItems = _contentService.Content.MenuItems,

                BetaBanner = _contentService.Content.BetaBanner,

                InterimFooterLinks = _contentService.Content.InterimFooterLinks

            };

        }

    }

}
