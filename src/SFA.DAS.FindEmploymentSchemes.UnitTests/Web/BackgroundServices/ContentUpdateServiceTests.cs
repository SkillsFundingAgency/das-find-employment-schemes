﻿using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.BackgroundServices
{
    public class ContentUpdateServiceTests
    {
        public ContentUpdateServiceOptions ContentUpdateServiceOptions { get; set; }
        public IOptions<ContentUpdateServiceOptions> ContentUpdateServiceOptionsOptions { get; set; }
        public IContentService ContentService { get; set; }
        public ILogger<ContentUpdateService> Logger { get; set; }

        public ContentUpdateServiceTests()
        {
            ContentUpdateServiceOptions = new ContentUpdateServiceOptions
            {
                Enabled = true,
                CronSchedule = "*/30 * * * *"
            };
            ContentUpdateServiceOptionsOptions = A.Fake<IOptions<ContentUpdateServiceOptions>>();

            A.CallTo(() => ContentUpdateServiceOptionsOptions.Value)
                .Returns(ContentUpdateServiceOptions);
            
            ContentService = A.Fake<IContentService>();
            Logger = A.Fake<ILogger<ContentUpdateService>>();
        }

        [Fact]
        public void Ctor_EmptyCronScheduleConfigTest()
        {
            ContentUpdateServiceOptions.CronSchedule = "";

            Assert.Throws<ContentUpdateServiceException>(CreateContentUpdateService);
        }

        [Fact]
        public void Ctor_NullCronScheduleConfigTest()
        {
            ContentUpdateServiceOptions.CronSchedule = null;

            Assert.Throws<ContentUpdateServiceException>(CreateContentUpdateService);
        }

        [Fact]
        public async Task StartAsync_NotEnabledDontUpdateTest()
        {
            ContentUpdateServiceOptions.Enabled = false;

            var contentUpdateService = CreateContentUpdateService();

            await contentUpdateService.StartAsync(CancellationToken.None);

            A.CallTo(() => ContentService.Update())
                .MustNotHaveHappened();
        }

        private ContentUpdateService CreateContentUpdateService()
        {
            return new ContentUpdateService(ContentUpdateServiceOptionsOptions, ContentService, Logger);
        }
    }
}
