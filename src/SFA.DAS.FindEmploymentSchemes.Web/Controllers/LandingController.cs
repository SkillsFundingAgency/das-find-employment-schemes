﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{

    public class LandingController : Controller
    {

        private readonly IInterimModelService _interimModelService;

        public LandingController(IInterimModelService interimModelService)
        {

            _interimModelService = interimModelService;

        }

        //[ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Landing()
        {

            LandingModel? landingModel = _interimModelService.GetLandingModel();

            return View(nameof(Landing), landingModel);

        }

        public async Task<IActionResult> LandingPreview()
        {

            LandingModel? landingModel = await _interimModelService.GetLandingPreviewModel();

            return View(nameof(Landing), landingModel);

        }

    }

}
