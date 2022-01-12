using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using Cronos;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using Contentful.Core;
using Contentful.Core.Search;

namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
    [Serializable]
    public class ContentUpdateServiceException : Exception
    {
        public ContentUpdateServiceException()
        {
        }

        protected ContentUpdateServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ContentUpdateServiceException(string? message) : base(message)
        {
        }

        public ContentUpdateServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    //todo: config from storage table

    /// <summary>
    /// Updates content on a schedule, given in a configurable cron expression.
    /// It uses a cron expression, as it's standard, well understood, supported by libraries and provides flexibility,
    /// but also because we want all instances of the app services to update as one,
    /// so the site isn't serving different versions of the content, depending on the instance that happens to service the request.
    /// </summary>
    public class ContentUpdateService : IHostedService, IDisposable
    {
        private int _executionCount;
        private readonly ILogger<ContentUpdateService> _logger;
        private readonly IContentfulClient _contentfulClient;
        private readonly IFilterService _filterService;
        private Timer? _timer;
        private readonly CronExpression _cronExpression;

        public ContentUpdateService(
            ILogger<ContentUpdateService> logger,
            //todo: create service to get content from contentful and inject that instead
            IContentfulClient contentfulClient,
            IFilterService filterService)
        {
            _logger = logger;
            _contentfulClient = contentfulClient;
            _filterService = filterService;
            //todo: from config
            //todo: do we want to support changing config without an app service restart?
            _cronExpression = CronExpression.Parse("* * * * *");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Content Update Service running.");

            var delay = TimeToNextInvocation();

            _timer = new Timer(UpdateContent, null, delay, Timeout.InfiniteTimeSpan);

            return Task.CompletedTask;
        }

        private TimeSpan TimeToNextInvocation()
        {
            var now = DateTime.UtcNow;
            DateTime? next = _cronExpression.GetNextOccurrence(now);
            if (next == null)
                throw new ContentUpdateServiceException("Next invocation time is unreachable.");

            return next.Value - now;
        }

        private void UpdateContent(object? state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            _logger.LogInformation("Content Update Service is updating content. Count: {Count}", count);

            var delay = TimeToNextInvocation();

            //todo: check timer null & throw ex?
            _timer!.Change(delay, Timeout.InfiniteTimeSpan);

            _filterService.HomeModel = new HomeModel(null!, null!, null!);
            // new ctor for homemodel that accepts pages and schemes?
                //SchemesContent.Pages.First(p => p.Url == HomepagePreambleUrl).Content,
                //SchemesContent.Schemes,
                //new[] {
                //    new FilterGroupModel(MotivationName, MotivationDescription, SchemesContent.MotivationsFilters),
                //    new FilterGroupModel(SchemeLengthName, SchemeLengthDescription, SchemesContent.SchemeLengthFilters),
                //    new FilterGroupModel(PayName, PayDescription, SchemesContent.PayFilters)
                //});

            Interlocked.Decrement(ref _executionCount);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Content Update is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _timer?.Dispose();
        }

        //private async Task x()
        //{
        //    //todo: wrap calling into service and use in generator and here
        //    var builder = QueryBuilder<Page>.New.ContentTypeIs("page");

        //    var pages = await _contentfulClient.GetEntries<Page>(builder);
        //}
    }
}
