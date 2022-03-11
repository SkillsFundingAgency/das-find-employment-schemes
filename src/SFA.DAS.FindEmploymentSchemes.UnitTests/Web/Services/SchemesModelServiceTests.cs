﻿using System.Collections.Generic;
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
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Collections;
using static SFA.DAS.FindEmploymentSchemes.UnitTests.Web.ViewModels.SchemeFilterViewModelTests;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class SchemesModelServiceTests
    {
        public Fixture Fixture { get; }
        public IEnumerable<Page> NotHomepages { get; set; }
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

            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(Content);

            NotHomepages = Fixture.CreateMany<Page>(3);
            var homePage = new Page("", "home", new HtmlString(HomePagePreamble));

            var pages = NotHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            var schemes = Fixture.CreateMany<Scheme>(3).ToArray();

            A.CallTo(() => Content.Schemes)
                .Returns(schemes);

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
            // act
            var model = await SchemesModelService.CreateHomeModelPreview();

            Assert.True(model.Preview.IsPreview);
        }

        [Fact]
        public async Task CreateHomeModelPreview_PreambleNull_PreviewErrorTest()
        {
            var homePage = new Page("", "home", null);

            var pages = NotHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            // act
            var model = await SchemesModelService.CreateHomeModelPreview();

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Preamble must not be blank", e.Value));
        }

        [Fact]
        public void GetSchemeDetailsModel_KnownSchemeUrl_ReturnsModelTest()
        {
            // act
            var model = SchemesModelService.GetSchemeDetailsModel(Content.Schemes.First().Url);

            Assert.NotNull(model);
        }

        [Fact]
        public void GetSchemeDetailsModel_UnknownSchemeUrl_ReturnsModelTest()
        {
            string unknownUrl = "unknownUrl";

            // act
            var model = SchemesModelService.GetSchemeDetailsModel(unknownUrl);

            Assert.Null(model);
        }

        [Fact]
        public async Task GetSchemeDetailsModelPreview__IsPreviewIsTrueTest()
        {
            // act
            var model = await SchemesModelService.GetSchemeDetailsModelPreview(Content.Schemes.First().Url);

            Assert.True(model.Preview.IsPreview);
        }

        [Theory]
        [ClassData(typeof(PreviewErrorTestData))]
        public async Task GetSchemeDetailsModelPreview_SingleMissingMandatoryField_PreviewErrorTest(string expected, Scheme scheme)
        {
            var schemes = new[] { scheme };

            A.CallTo(() => Content.Schemes)
                .Returns(schemes);

            // act
            var model = await SchemesModelService.GetSchemeDetailsModelPreview(Content.Schemes.First().Url);

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal(expected, e.Value));
        }

        public class PreviewErrorTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "Name must not be blank", new Scheme(null, new HtmlString("shortDescription"), new HtmlString("shortCost"), new HtmlString("shortBenefits"), new HtmlString("shortTime"), "url", 0, Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        //todo: test to ensure content from UpdatePreview is used for preview gets
    }
}
