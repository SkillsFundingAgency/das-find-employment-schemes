
using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Employer.FrontDoor.Web.Infrastructure;


namespace SFA.DAS.FAT.Web.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _log;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _log = logger;
        }

        [Route("404", Name = RouteNames.Error404)]
        public IActionResult PageNotFound()
        {            
            return View();
        }

        [Route("500", Name = RouteNames.Error500)]
        public IActionResult ApplicationError()
        {
            IExceptionHandlerPathFeature? feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _log.LogError($"500 result at {feature?.Path ?? "{unknown}"}", feature?.Error);
            return View();
        }
    }
}
