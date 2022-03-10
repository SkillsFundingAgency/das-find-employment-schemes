//using System;
//using System.Collections.Generic;
//using AutoFixture;
//using AutoFixture.Kernel;
//using Contentful.Core.Models;
//using FakeItEasy;
//using SFA.DAS.FindEmploymentSchemes.Web.Services;
//using System.Linq;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
//using Xunit;
//using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;

//namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
//{
//    public class CaseStudyPageServiceTests
//    {
//        public Fixture Fixture { get; set; }
//        public IEnumerable<CaseStudyPage> CaseStudyPages { get; set; }
//        public IContent Content { get; set; }
//        public IContentService ContentService { get; set; }
//        public CaseStudyPageService CaseStudyPageService { get; set; }

//        public CaseStudyPageServiceTests()
//        {
//            Fixture = new Fixture();

//            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//                .ForEach(b => Fixture.Behaviors.Remove(b));
//            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

//            Fixture.Customizations.Add(
//                new TypeRelay(
//                    typeof(global::Contentful.Core.Models.IContent),
//                    typeof(Paragraph)));

//            Content = A.Fake<IContent>();
//            ContentService = A.Fake<IContentService>();

//            CaseStudyPages = Fixture.CreateMany<CaseStudyPage>(3);

//            A.CallTo(() => Content.CaseStudyPages)
//                .Returns(CaseStudyPages);

//            CaseStudyPageService = new CaseStudyPageService(ContentService);
//        }

//        [Fact]
//        public void CaseStudyPage_StandardPageUrlReturnsDefaultViewNameTest()
//        {
//            string caseStudyPageUrl = CaseStudyPages.Skip(1).First().Url;

//            var (viewName, caseStudyPage) = CaseStudyPageService.CaseStudyPage(caseStudyPageUrl, Content);

//            Assert.Null(viewName);
//        }

//        [Fact]
//        public void CaseStudyPage_StandardPageUrlReturnsPageTest()
//        {
//            CaseStudyPage expectedPage = CaseStudyPages.Skip(1).First();

//            var (viewName, caseStudyPage) = CaseStudyPageService.CaseStudyPage(expectedPage.Url, Content);

//            Assert.Equal(expectedPage, caseStudyPage);
//        }

//        [Fact]
//        public void Page_UnknownPageUrlReturnsNullPageTest()
//        {
//            string unknowUrl = nameof(unknowUrl);

//            var (viewName, caseStudyPage) = CaseStudyPageService.CaseStudyPage(unknowUrl, Content);

//            Assert.Null(caseStudyPage);
//        }

//        [Fact]
//        public void Page_ErrorCheckPageUrlThrowsExceptionTest()
//        {
//            string errorCheckUrl = "error-check";

//            Assert.Throws<NotImplementedException>(() => CaseStudyPageService.CaseStudyPage(errorCheckUrl, Content));
//        }
//    }
//}
