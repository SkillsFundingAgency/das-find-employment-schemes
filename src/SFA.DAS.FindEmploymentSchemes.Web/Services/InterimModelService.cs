using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class InterimModelService : IInterimModelService
    {

        #region Properties

        private readonly ILogger<InterimModelService> _logger;

        private readonly IContentService _contentService;

        public LandingModel? LandingModel { get; set; }

        #endregion

        #region Constructors

        public InterimModelService(ILogger<InterimModelService> logger, IContentService contentService) 
        {

            _logger = logger;

            _contentService = contentService;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the interim page model containing data associated with the specified interim URL.
        /// </summary>
        /// <param name="interimURL">The URL of the interim page.</param>
        /// <returns>Returns the interim page model if available, otherwise returns null.</returns>
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

        /// <summary>
        /// Retrieves the landing model containing data from various content sources.
        /// </summary>
        /// <returns>Returns the landing model if available, otherwise returns null.</returns>
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

        #region Preview Models

        public async Task<InterimPageModel?> GetInterimPagePreviewModel(string interimURL)
        {

            try
            {

                IContent previewContent = await _contentService.UpdatePreview();

                InterimPage? interimPage = _contentService.GetPreviewInterimPageByURL(interimURL);
                    
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

                    MenuItems = previewContent.MenuItems,

                    BetaBanner = previewContent.BetaBanner,

                    InterimFooterLinks = previewContent.InterimFooterLinks,

                    Preview = new PreviewModel(Enumerable.Empty<HtmlString>())

                };

            }
            catch (Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get interim page.");

                return null;

            }

        }

        public async Task<LandingModel?> GetLandingPreviewModel()
        {

            try
            {

                IContent previewContent = await _contentService.UpdatePreview();

                InterimLandingPage? landingPage = previewContent.InterimLandingPage;

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

                    MenuItems = previewContent.MenuItems,

                    BetaBanner = previewContent.BetaBanner,

                    InterimFooterLinks = previewContent.InterimFooterLinks,

                    Preview = new PreviewModel(Enumerable.Empty<HtmlString>())

                };

            }
            catch (Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get interim preview landing page.");

                return null;

            }

        }

        #endregion

        #endregion

    }

}
