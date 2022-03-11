using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class SchemesModelServiceTests
    {
        public Fixture Fixture { get; }
        public IContent Content { get; set; }
        public IContentService ContentService { get; set; }
        public SchemesModelService SchemesModelService { get; set; }

        public const string HomePagePreamble = "expectedPreamble";

        public SchemesModelServiceTests()
        {
            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IContent),
                    typeof(Paragraph)));

            ContentService = A.Fake<IContentService>();
            Content = A.Fake<IContent>();

            A.CallTo(() => ContentService.Content)
                .Returns(Content);

            var notHomepages = Fixture.CreateMany<Page>(3);
            var homePage = new Page("", "home", new HtmlString(HomePagePreamble));

            var pages = notHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            SchemesModelService = new SchemesModelService(ContentService);
        }

        [Fact]
        public void HomeModel_HomepagePreambleTest()
        {
            Assert.IsType<HtmlString>(SchemesModelService.HomeModel.Preamble);
            Assert.Equal(HomePagePreamble, ((HtmlString) SchemesModelService.HomeModel.Preamble).Value);
        }

        [Fact]
        public void ContentServiceContentUpdated_UpdatesHomeModel()
        {
            const string updatedPreamble = "updatedPreamble";

            SchemesModelService = new SchemesModelService(ContentService);

            var notHomepages = Fixture.CreateMany<Page>(3);
            var homePage = new Page("", "home", new HtmlString(updatedPreamble));

            var pages = notHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            // Act
            ContentService.ContentUpdated += Raise.WithEmpty();

            Assert.IsType<HtmlString>(SchemesModelService.HomeModel.Preamble);
            Assert.Equal(updatedPreamble, ((HtmlString)SchemesModelService.HomeModel.Preamble).Value);
        }

        [Fact]
        public async Task CreateHomeModelPreview_IsPreviewIsTrueTest()
        {
            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            // act
            var model = await SchemesModelService.CreateHomeModelPreview();

            Assert.True(model.Preview.IsPreview);
        }
    }
}
