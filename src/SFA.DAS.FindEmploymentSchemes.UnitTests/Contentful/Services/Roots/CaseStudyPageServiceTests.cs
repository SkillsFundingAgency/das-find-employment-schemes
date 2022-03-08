using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using FakeItEasy;
using System;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class CaseStudyPageServiceTests : RootServiceTestBase
    {
        public ContentfulCollection<CaseStudyPage> CaseStudyPagesCollection { get; set; }
        public ILogger<CaseStudyPageService> Logger { get; set; }
        public ICaseStudyPageService CaseStudyPageService { get; set; }

        public CaseStudyPageServiceTests()
        {
            CaseStudyPagesCollection = new ContentfulCollection<CaseStudyPage> { Items = Array.Empty<CaseStudyPage>() };
            SetupContentfulClientCall(ContentfulClient, CaseStudyPagesCollection);

            Logger = A.Fake<ILogger<CaseStudyPageService>>();

            CaseStudyPageService = new CaseStudyPageService(HtmlRenderer, Logger);
        }
    }
}
