using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class SchemeServiceTests : RootServiceTestBase<Page, SchemeService>
    {
        public SchemeService SchemeService { get; set; }

        public SchemeServiceTests()
        {
            SchemeService = new SchemeService(HtmlRenderer, Logger);
        }
    }
}
