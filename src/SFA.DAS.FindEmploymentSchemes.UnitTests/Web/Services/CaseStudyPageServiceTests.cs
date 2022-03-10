using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class CaseStudyPageServiceTests
    {
        public Fixture Fixture { get; set; }
        public IEnumerable<CaseStudyPage> CaseStudyPages { get; set; }
        public IContent Content { get; set; }
        public IContentService ContentService { get; set; }
        public CaseStudyPageService CaseStudyPageService { get; set; }

        public CaseStudyPageServiceTests()
        {
            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(global::Contentful.Core.Models.IContent),
                    typeof(Paragraph)));

            Content = A.Fake<IContent>();
            ContentService = A.Fake<IContentService>();

            CaseStudyPages = Fixture.CreateMany<CaseStudyPage>(3);

            A.CallTo(() => Content.CaseStudyPages)
                .Returns(CaseStudyPages);

            CaseStudyPageService = new CaseStudyPageService(ContentService);
        }

        //todo: need more tests

        [Fact]
        public void GetCaseStudyPageModel_ErrorCheckPageUrlThrowsExceptionTest()
        {
            string errorCheckUrl = "error-check";

            Assert.Throws<NotImplementedException>(() => CaseStudyPageService.GetCaseStudyPageModel(errorCheckUrl));
        }
    }
}
