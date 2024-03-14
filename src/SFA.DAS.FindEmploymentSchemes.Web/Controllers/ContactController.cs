using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {

            _contactService = contactService;

        }

        [Route("index")]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {

            return View(_contactService.GetContactPageModel());

        }

        [Route("preview/index")]
        public async Task<IActionResult> IndexPreview()
        {

            return View(

                "Index",

                await _contactService.GetContactPreviewPageModel()
                
            );

        }

    }

}
