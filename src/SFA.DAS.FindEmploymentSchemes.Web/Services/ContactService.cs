using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class ContactService : IContactService
    {

        #region Properties

        private readonly IContentService _contentService;

        #endregion

        #region Constructors

        #pragma warning disable CS8618
        public ContactService(IContentService contentService)
        {

            _contentService = contentService;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the contact page model containing contact-related data.
        /// </summary>
        /// <returns>Returns the contact page model if available, otherwise returns null.</returns>
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

        public async Task<ContactPageModel?> GetContactPreviewPageModel()
        {

            await _contentService.UpdatePreview();

            var previewContent = _contentService.PreviewContent ?? _contentService.Content;

            if (previewContent?.ContactPage == null)
            {

                return null;

            }

            return new ContactPageModel()
            {

                ContactPageTitle = previewContent.ContactPage.ContactPageTitle,

                InterimBreadcrumbs = previewContent.ContactPage.InterimBreadcrumbs,

                InterimPreamble = previewContent.ContactPage.InterimPreamble,

                Contacts = previewContent.ContactPage.Contacts,

                MenuItems = previewContent.MenuItems,

                BetaBanner = previewContent.BetaBanner,

                InterimFooterLinks = previewContent.InterimFooterLinks

            };

        }

        #endregion

    }

}
