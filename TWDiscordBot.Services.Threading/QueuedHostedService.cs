using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TWDiscordBot.Services.Threading.Contracts;

namespace TWDiscordBot.Services.Threading
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger _logger;

        public IBackgroundTaskQueue TaskQueue { get; }

        public event EventHandler TaskFinished;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue,
            ILoggerFactory loggerFactory)
        {
            TaskQueue = taskQueue;
            _logger = loggerFactory.CreateLogger<QueuedHostedService>();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                    OnTaskFinished(EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"Error occurred executing {nameof(workItem)}.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }

        protected virtual void OnTaskFinished(EventArgs e)
        {
            var handler = TaskFinished;
            handler?.Invoke(this, e);
        }
    }
}
