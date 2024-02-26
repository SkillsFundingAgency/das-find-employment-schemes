using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class InterimModelService : IInterimModelService
    {

        private readonly ILogger<InterimModelService> _logger;

        private readonly IContentService _contentService;

        public LandingModel? LandingModel { get; set; }

        public InterimModelService(ILogger<InterimModelService> logger, IContentService contentService) 
        {

            _logger = logger;

            _contentService = contentService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interimURL"></param>
        /// <returns></returns>
        public InterimPageModel? GetInterimPageModel(string interimURL)
        {

            try
            {

                InterimPage? interimPage = _contentService.GetInterimPageByURL(interimURL);

                if (interimPage == null)
                {

                    return null;

                }

                return new InterimPageModel()
                {

                    InterimPageTitle = interimPage.InterimPageTitle,

                    InterimPageURL = interimPage.InterimPageURL,

                    InterimPagePreamble = interimPage.InterimPagePreamble,

                    InterimPageComponents = interimPage.InterimPageComponents,

                    InterimPageTileSections = interimPage.InterimPageTileSections,

                    InterimPageBreadcrumbs = interimPage.InterimPageBreadcrumbs,

                    MenuItems = _contentService.Content.MenuItems,

                    BetaBanner = _contentService.Content.BetaBanner,

                    InterimFooterLinks = _contentService.Content.InterimFooterLinks

                };

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get interim page.");

                return null;

            }

        }

        public LandingModel? GetLandingModel()
        {

            try
            {

                InterimLandingPage? landingPage = _contentService.Content.InterimLandingPage;

                if (landingPage == null)
                {

                    return null;

                }

                return new LandingModel()
                {

                    InterimLandingPageID = landingPage.InterimLandingPageID,

                    InterimLandingPageTitle = landingPage.InterimLandingPageTitle,

                    InterimLandingPagePreamble = landingPage.InterimLandingPagePreamble,

                    InterimTileSections = landingPage.InterimTileSections,

                    MenuItems = _contentService.Content.MenuItems,

                    BetaBanner = _contentService.Content.BetaBanner,

                    InterimFooterLinks = _contentService.Content.InterimFooterLinks

                };

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get interim landing page.");

                return null;

            }

        }

    }

}
