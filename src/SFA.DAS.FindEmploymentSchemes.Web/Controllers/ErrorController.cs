using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Infrastructure;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _log;

        private static readonly LayoutModel LayoutModel = new LayoutModel();

        private readonly IContentService _contentService;

        public ErrorController(ILogger<ErrorController> logger, IContentService contentService)
        {

            _log = logger;

            _contentService = contentService;

        }

        [Route("404", Name = RouteNames.Error404)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PageNotFound()
        {

            try
            {
                LayoutModel.InterimFooterLinks = _contentService.Content.InterimFooterLinks;

                return View(LayoutModel);

            }
            catch (Exception _exception)
            {

                _log.LogError(_exception, "Unable to get model with populated footer");

                return View(LayoutModel);

            }

        }

        [Route("500", Name = RouteNames.Error500)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ApplicationError()
        {
            IExceptionHandlerPathFeature? feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _log.LogError($"500 result at {feature?.Path ?? "{unknown}"}", feature?.Error);
            return View(LayoutModel);
        }
    }
}
