using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{

    public class InterimPageController : Controller
    {

        private readonly IInterimModelService _interimModelService;

        public InterimPageController(IInterimModelService interimModelService)
        {

            _interimModelService = interimModelService;

        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult InterimPage(string interimURL)
        {

            
            InterimPageModel? interimPageModel = _interimModelService.GetInterimPageModel(interimURL);

            if(interimPageModel == null)
            {

                return NotFound();

            }

            return View(nameof(InterimPage), interimPageModel);

        }

    }

}
