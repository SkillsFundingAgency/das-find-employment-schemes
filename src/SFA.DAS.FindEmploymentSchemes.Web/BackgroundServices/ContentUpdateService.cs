using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;
using Cronos;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
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
        private readonly IContentService _contentService;
        private Timer? _timer;
        private readonly CronExpression _cronExpression;

        public ContentUpdateService(
            ILogger<ContentUpdateService> logger,
            IContentService contentService)
        {
            _logger = logger;
            _contentService = contentService;
            //todo: from config
            //todo: do we want to support changing config without an app service restart?
            _cronExpression = CronExpression.Parse("* * * * *");
        }

        //todo: page with content version?
        //todo: logging

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Content Update Service running.");

            try
            {
                await _contentService.Update();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "Initial content update content!");
            }

            var delay = TimeToNextInvocation();

            //todo: want to update content straight away, then timer
            _timer = new Timer(UpdateContent, null, delay, Timeout.InfiniteTimeSpan);
        }

        private TimeSpan TimeToNextInvocation()
        {
            var now = DateTime.UtcNow;
            DateTime? next = _cronExpression.GetNextOccurrence(now);
            if (next == null)
                throw new ContentUpdateServiceException("Next invocation time is unreachable.");

            return next.Value - now;
        }

        // event handler, so ok to use async void, as per sonar's/asyncfixer's warning descriptions (and also given the thumbs up by Stephen Clearly)
        // we also catch and consume all exceptions
#pragma warning disable S3168
#pragma warning disable AsyncFixer03
        private async void UpdateContent(object? state)
        {
            try
            {
                var count = Interlocked.Increment(ref _executionCount);

                _logger.LogInformation("Content Update Service is updating content. Count: {Count}", count);

                var delay = TimeToNextInvocation();

                //todo: check timer null & throw ex?
                _timer!.Change(delay, Timeout.InfiniteTimeSpan);

                await _contentService.Update();

                //todo: update event in sitemap

                Interlocked.Decrement(ref _executionCount);
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "Update content failed!");
            }
        }
#pragma warning restore AsyncFixer03
#pragma warning restore S3168

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
    }
}
