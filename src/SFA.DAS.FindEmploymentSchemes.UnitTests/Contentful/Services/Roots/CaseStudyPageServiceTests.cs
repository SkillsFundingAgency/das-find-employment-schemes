using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using FakeItEasy;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class CaseStudyPageServiceTests : RootServiceTestBase<CaseStudyPage>
    {
        public ILogger<CaseStudyPageService> Logger { get; set; }
        public ICaseStudyPageService CaseStudyPageService { get; set; }

        public CaseStudyPageServiceTests()
        {
            Logger = A.Fake<ILogger<CaseStudyPageService>>();

            CaseStudyPageService = new CaseStudyPageService(HtmlRenderer, Logger);
        }
    }
}
