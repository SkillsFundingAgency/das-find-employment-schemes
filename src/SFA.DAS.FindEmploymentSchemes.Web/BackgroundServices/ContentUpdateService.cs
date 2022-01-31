using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;
using Cronos;
using Microsoft.Extensions.Options;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
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
        private readonly bool _enabled;
        private readonly CronExpression _cronExpression;

        public ContentUpdateService(
            IOptions<ContentUpdateServiceOptions> contentUpdateServiceOptions,
            IContentService contentService,
            ILogger<ContentUpdateService> logger)
        {
            _logger = logger;
            _contentService = contentService;

            // for envs: every half "*/30 * * * *"
            // for debugging : once a minute "* * * * *"
            var options = contentUpdateServiceOptions.Value;

            _enabled = options.Enabled;

            if (options.CronSchedule == null)
                throw new ContentUpdateServiceException("ContentUpdates:CronSchedule config is missing.");

            _cronExpression = CronExpression.Parse(options.CronSchedule);
        }

        //todo: page with content version?

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Content Update Service running.");

            if (!_enabled)
            {
                _logger.Log(LogLevel.Information, "Online content updates disabled.");
                return;
            }

            try
            {
                await _contentService.Update();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "Initial content update failed!");
            }

            var delay = TimeToNextInvocation();

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

                _timer!.Change(delay, Timeout.InfiniteTimeSpan);

                //todo: how to handle content update ok, but event handler throwing?
                await _contentService.Update();

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
