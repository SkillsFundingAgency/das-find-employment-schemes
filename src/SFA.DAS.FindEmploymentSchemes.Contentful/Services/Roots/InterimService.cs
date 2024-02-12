using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{

    public class InterimService : ContentRootService, IInterimService
    {

        private readonly ILogger<InterimService> _logger;

        public InterimService(HtmlRenderer htmlRenderer, ILogger<InterimService> logger) : base(htmlRenderer)
        {

            _logger = logger;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentfulClient"></param>
        /// <returns></returns>
        public async Task<InterimLandingPage?> GetLandingPage(IContentfulClient contentfulClient)
        {

            _logger.LogInformation("Beginning {MethodName}...", nameof(GetLandingPage));

            try
            {

                var query = new QueryBuilder<InterimLandingPage>()

                    .FieldEquals(page => page.InterimLandingPageID, "interim-landing-page")

                    .ContentTypeIs("interimLandingPage")

                    .Include(2);

                var results = await contentfulClient.GetEntries(query);

                List<InterimLandingPage> resultList = results.Items.ToList();

                if (resultList.Any())
                {

                    InterimLandingPage landingPage = resultList[0];

                    _logger.LogInformation("Retrieved landing page: {Title}", landingPage.InterimLandingPageTitle);

                    return landingPage;

                }
                else
                {

                    _logger.LogInformation("No matching landing pages found.");

                    return null;

                }

            }
            catch(Exception _Exception)
            {

                _logger.LogError(_Exception, "Unable to get the interim landing page.");

                return null;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentfulClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InterimMenuItem>> GetMenuItems(IContentfulClient contentfulClient)
        {

            _logger.LogInformation("Beginning {MethodName}...", nameof(GetMenuItems));

            try
            {

                var builder = QueryBuilder<InterimMenuItem>.New.ContentTypeIs("interimMenuItem");

                var menuItems = await contentfulClient.GetEntries(builder);

                if (menuItems.Any())
                {

                    _logger.LogInformation("Retrieved landing page: {MenuItemCount}", menuItems.Count());

                    return menuItems.OrderBy(t => t.InterimMenuItemOrder);

                }
                else
                {

                    _logger.LogInformation("No interim menu items found.");

                    return Enumerable.Empty<InterimMenuItem>();

                }

            }
            catch (Exception _Exception)
            {

                _logger.LogError(_Exception, "Unable to get the interim menu items.");

                return Enumerable.Empty<InterimMenuItem>();

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentfulClient"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InterimPage>> GetInterimPages(IContentfulClient contentfulClient)
        {

            _logger.LogInformation("Beginning {MethodName}...", nameof(GetInterimPages));

            try
            {

                var builder = QueryBuilder<InterimPage>.New.ContentTypeIs("interimPage").Include(2);

                var interimPages = await contentfulClient.GetEntries(builder);

                if (interimPages.Any())
                {

                    _logger.LogInformation("Retrieved interim pages: {MenuItemCount}", interimPages.Count());

                    return interimPages;

                }
                else
                {

                    _logger.LogInformation("No interim pages found.");

                    return Enumerable.Empty<InterimPage>();

                }

            }
            catch (Exception _Exception)
            {

                _logger.LogError(_Exception, "Unable to get the interim pages.");

                return Enumerable.Empty<InterimPage>();

            }

        }

    }

}
