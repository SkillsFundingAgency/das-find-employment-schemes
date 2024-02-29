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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Contentful.Core.Models.Management;

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

            Fixture.Customizations.Add(
            new TypeRelay(
            typeof(IFieldValidator),
            typeof(Asset)));

            Content = A.Fake<IContent>();
            ContentService = A.Fake<IContentService>();

            CaseStudyPages = Fixture.CreateMany<CaseStudyPage>(3);

            A.CallTo(() => Content.CaseStudyPages)
                .Returns(CaseStudyPages);

            CaseStudyPageService = new CaseStudyPageService(ContentService);
        }

        [Fact]
        public void GetCaseStudyPageModel_ErrorCheckPageUrlThrowsExceptionTest()
        {
            string errorCheckUrl = "error-check";

            Assert.Throws<NotImplementedException>(() => CaseStudyPageService.GetCaseStudyPageModel(errorCheckUrl));
        }

        [Fact]
        public async Task GetCaseStudyPageModelPreview_IsPreviewIsTrueTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            // act
            var model = await CaseStudyPageService.GetCaseStudyPageModelPreview(CaseStudyPages.First().Url);

            Assert.True(model.Preview.IsPreview);
        }

        [Fact]
        public async Task GetCaseStudyPageModelPreview_ContentNull_PreviewErrorTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            var caseStudyPage = new CaseStudyPage("title", "url", CaseStudyPages.First().Scheme, null);

            var caseStudyPages = new[] { caseStudyPage };

            A.CallTo(() => Content.CaseStudyPages)
                .Returns(caseStudyPages);

            // act
            var model = await CaseStudyPageService.GetCaseStudyPageModelPreview(caseStudyPage.Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Content must not be blank", e.Value));
        }

        [Fact]
        public async Task GetCaseStudyPageModelPreview_TitleNull_PreviewErrorTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            var caseStudyPage = new CaseStudyPage(null, "url", CaseStudyPages.First().Scheme, new HtmlString("content"));

            var caseStudyPages = new[] { caseStudyPage };

            A.CallTo(() => Content.CaseStudyPages)
                .Returns(caseStudyPages);

            // act
            var model = await CaseStudyPageService.GetCaseStudyPageModelPreview(caseStudyPage.Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Title must not be blank", e.Value));
        }

        [Fact]
        public async Task GetCaseStudyPageModelPreview_SchemeNull_PreviewErrorTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            var caseStudyPage = new CaseStudyPage("title", "url", null, new HtmlString("content"));

            var caseStudyPages = new[] { caseStudyPage };

            A.CallTo(() => Content.CaseStudyPages)
                .Returns(caseStudyPages);

            // act
            var model = await CaseStudyPageService.GetCaseStudyPageModelPreview(caseStudyPage.Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Scheme must be selected and have been given an URL and name before publishing", e.Value));
        }
    }
}
