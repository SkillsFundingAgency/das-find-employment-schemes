using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models.Management;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ContentScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Scheme;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class CaseStudyPageServiceTests : RootServiceTestBase<CaseStudyPage, CaseStudyPageService>
    {
        public IEnumerable<ContentScheme> ContentSchemes { get; set; }
        public ICaseStudyPageService CaseStudyPageService { get; set; }

        public CaseStudyPageServiceTests()
        {
            CaseStudyPageService = new CaseStudyPageService(HtmlRenderer, Logger);

            Fixture.Customizations.Add(
            new TypeRelay(
            typeof(IFieldValidator),
            typeof(Asset)));

        }

        [Fact]
        public async Task GetAll_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            ContentSchemes = Fixture.CreateMany<ContentScheme>(1);
            var contentScheme = ContentSchemes.First();

            var apiScheme = Fixture.Create<Scheme>();
            apiScheme.Url = contentScheme.Url;

            Fixture.Inject(apiScheme);
            ContentfulCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var caseStudyPages = await CaseStudyPageService.GetAll(ContentfulClient, ContentSchemes);

            Assert.NotNull(caseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, caseStudyPages.Count());
        }

        [Fact]
        public async Task GetAll_CaseStudyPageMappedTest()
        {
            const int numberOfCaseStudyPages = 1;

            ContentSchemes = Fixture.CreateMany<ContentScheme>(1);
            var contentScheme = ContentSchemes.First();

            var apiScheme = Fixture.Create<Scheme>();
            apiScheme.Url = contentScheme.Url;

            Fixture.Inject(apiScheme);
            ContentfulCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var caseStudyPages = await CaseStudyPageService.GetAll(ContentfulClient, ContentSchemes);

            var actualCaseStudyPage = caseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = ContentfulCollection.Items.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            Assert.Equal(contentScheme.Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent.Value, actualCaseStudyPage.Content.Value);
        }
    }
}
