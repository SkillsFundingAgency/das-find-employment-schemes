using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class SchemesModelServiceTests
    {
        public Fixture Fixture { get; }
        public IContent Content { get; set; }
        public IContentService ContentService { get; set; }
        public SchemesModelService SchemesModelService { get; set; }

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
        }

        [Fact]
        public void HomeModel_HomepagePreambleTest()
        {
            const string expectedPreamble = "expectedPreamble";

            var notHomepages = Fixture.CreateMany<Page>(3);
            var homePage = new Page("", "home", new HtmlString("expectedPreamble"));

            var pages = notHomepages.Concat(new [] {homePage}).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            SchemesModelService = new SchemesModelService(ContentService);

            Assert.IsType<HtmlString>(SchemesModelService.HomeModel.Preamble);
            Assert.Equal(expectedPreamble, ((HtmlString) SchemesModelService.HomeModel.Preamble).Value);
        }
    }
}
