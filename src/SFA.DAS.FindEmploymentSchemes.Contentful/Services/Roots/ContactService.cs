using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{

    public class ContactService : ContentRootService, IContactService
    {

        #region Properties

        private readonly ILogger<ContactService>? _logger;

        #endregion

        #region Constructors

        public ContactService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {



        }

        public ContactService(ILogger<ContactService> logger, HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {

            _logger = logger;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the contact page from the contentful service.
        /// </summary>
        /// <param name="contentfulClient">Service with a set of methods to call underlying api endpoints at contentful</param>
        /// <returns>System.Collection.Generic.IEnumerable of type SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Contact.</returns>
        public async Task<ContactPage?> GetContactPage(IContentfulClient contentfulClient)
        {

            _logger?.LogInformation("Beginning {MethodName}...", nameof(GetContactPage));

            try
            {
                _logger?.LogInformation("Getting Contentful entries for content type: {ContentType}", "contactPage");

                var query = new QueryBuilder<ContactPage>()

                    .ContentTypeIs("contactPage")

                    .Include(3);

                var results = await contentfulClient.GetEntries(query);

                List<ContactPage> resultList = results.Items.ToList();

                if (resultList.Any())
                {

                    ContactPage contactPage = resultList[0];

                    _logger?.LogInformation("Retrieved contact page: {Title}", contactPage.ContactPageTitle);

                    return contactPage;

                }
                else
                {

                    _logger?.LogInformation("No matching contact page found.");

                    return null;

                }

            }
            catch(Exception _Exception)
            {

                _logger?.LogError(_Exception, "Unable to get the contact page.");

                return null;

            }

        }

        #endregion

    }

}
