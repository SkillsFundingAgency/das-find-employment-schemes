using AutoFixture;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model;
using SFA.DAS.FindEmploymentSchemes.Web.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{

    public class InterimPageServiceTests
    {

        public Fixture _fixture { get; set; }

        private string TestHtml = @"<div data-id='id' data-type='@Model.ComponentType' class='govuk-grid-column-full govuk-!-margin-bottom-7 govuk-!-padding-3' style='border: '1px solid #F8F8F8; background-color: #F8F8F8; border-radius: 20px'><div class='govuk-grid-row govuk-!-padding-1 govuk-!-padding-right-7'><div class='govuk-grid-column-full'><h2 class='govuk-heading-l'>Test Heading</h2></div></div></div>";

        public InterimPageServiceTests()
        {

            _fixture = new Fixture();

        }

        [Fact(DisplayName = "InterimPageServiceTest - InterimPageService - GenerateSubComponent - Correct sub component")]
        public void InterimPageServiceTests_InterimPageService_GenerateSubComponent()
        {

            InterimPageComponent component = new InterimPageComponent()
            {

                ComponentType = "interim-container",

                ComponentHeading = "Test Heading",

                ComponentID = "id"

            };

            IViewRenderService viewRenderService = A.Fake<IViewRenderService>();

            //InterimPageService.

            //viewRenderService.RenderToStringAsync();

            string componentHTML = InterimPageService.GenerateSubComponent(component);

            Assert.Equal(TestHtml, componentHTML);

        }

    }

}
