using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            switch (pageUrl)
            {
                case "cookies":
                    return View("Cookies");
                case "accessibility-statement":
                    return View("AccessibilityStatement");
            }

            var page = SchemesContent.Pages.FirstOrDefault(p => p.Url == pageUrl);
            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
