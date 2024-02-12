using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{

    public class ContactControllerTests
    {

        private readonly IContactService _ContactService;

        private readonly ContactController _ContactController;

        public ContactControllerTests()
        {

            _ContactService = A.Fake<IContactService>();

            _ContactController = new ContactController(_ContactService);

            A.CallTo(() => _ContactService.GetContactPageModel())

                .Returns(
                
                    new ContactPageModel()
                    {

                        ContactPageTitle = "Title",

                        Contacts = new List<Contact>()
                        {

                            new Contact()
                            {

                                SectionName = "SectionName",

                                Order = 1

                            }

                        }

                    }
                    
            );

        }

        [Fact(DisplayName = "ContactControllerTests - Index - Test")]
        public void ContactControllerTests_Index_Test()
        {

            IActionResult result = _ContactController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Single(((ContactPageModel)viewResult.Model).Contacts);

        }

    }

}
