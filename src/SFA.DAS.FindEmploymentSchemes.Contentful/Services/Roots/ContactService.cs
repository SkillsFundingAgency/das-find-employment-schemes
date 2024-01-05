using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{

    public class ContactService : ContentRootService, IContactService
    {
        #region Constructors

        public ContactService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all contacts from the contentful service.
        /// </summary>
        /// <param name="contentfulClient">Service with a set of methods to call underlying api endpoints at contentful</param>
        /// <returns>System.Collection.Generic.IEnumerable of type SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Contact.</returns>
        public async Task<IEnumerable<Contact>> GetContacts(IContentfulClient contentfulClient)
        {

            var builder = QueryBuilder<Contact>.New.ContentTypeIs("contact").Include(1);

            var contacts = await contentfulClient.GetEntries(builder);

            LogErrors(contacts);

            return contacts.OrderBy(t => t.Order);

        }

        #endregion

    }

}
