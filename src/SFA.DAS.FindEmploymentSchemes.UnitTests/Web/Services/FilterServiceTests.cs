﻿using FakeItEasy;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {

        private readonly BetaBanner BetaBanner;

        private static readonly string[] FilterAspects = new string[] { "aspect" };

        private static readonly string recruitNewStaff = "motivation--recruit-new-staff";
        
        private static readonly string sixAndOneYear = "duration--between-6-months-and-1-year";
        
        private static readonly string longerThanOneYear = "duration--longer-than-1-year";
        
        private static readonly string free = "cost--free";

        public FilterServiceTests()
        {

            BetaBanner = new BetaBanner() { BetaBannerID = "BetaBannerID", BetaBannerTitle = "BetaBannerTitle", BetaBannerContent = null };

        }

        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public async Task ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterModel filters)
        {
            IContentService contentService = A.Fake<IContentService>();
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();
            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);
            HomeModel model = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(expectedSchemes, Enumerable.Empty<FilterSectionModel>(), Enumerable.Empty<InterimMenuItem>(), null, null, BetaBanner, null, false, "")));
            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = await service.ApplyFilter(filters);
            Assert.True(expectedSchemes.Count() == result.Schemes.Count());
        }

        [Fact(DisplayName = "FilterService - ApplyFilters returns correct filter URL")]
        public async Task FilterService_ApplyFilters_Returns_Correct_Filter_URL()
        {

            IContentService contentService = A.Fake<IContentService>();

            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);

            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = await service.ApplyFilter(
                
                new SchemeFilterModel()
            
                {

                    FilterAspects = FilterAspects

                }
            
            );

            Assert.Equal("filters=aspect", result.SelectedFilters);

        }

        public class FilterServiceTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {

                var generatedContent = new GeneratedContent();

                yield return new object[] {
                    generatedContent.Schemes,
                    new SchemeFilterModel()
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(sixAndOneYear)),
                    new SchemeFilterModel { FilterAspects = new[] { sixAndOneYear }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(longerThanOneYear)),
                    new SchemeFilterModel { FilterAspects = new[] { longerThanOneYear }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(free)),
                    new SchemeFilterModel { FilterAspects = new[] { free }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(longerThanOneYear) && s.FilterAspects.Contains(free)),
                    new SchemeFilterModel { FilterAspects = new[] { longerThanOneYear, free }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(recruitNewStaff) && s.FilterAspects.Contains(free)),
                    new SchemeFilterModel { FilterAspects = new[] { recruitNewStaff, free }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(sixAndOneYear) || s.FilterAspects.Contains(longerThanOneYear)),
                    new SchemeFilterModel { FilterAspects = new[] { sixAndOneYear, longerThanOneYear }}
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}