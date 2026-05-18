using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;

namespace Arquipoza.Module.Tasks
{
    [BackgroundTask(
        Schedule = "0 2 * * *",
        Description = "Daily cleanup task for Arquipoza projects cache and logs."
    )]
    public class ProjectCleanupTask : IBackgroundTask
    {
        private readonly ILogger<ProjectCleanupTask> _logger;

        public ProjectCleanupTask(ILogger<ProjectCleanupTask> logger)
        {
            _logger = logger;
        }

        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Arquipoza background task running at {Time}. Performing daily maintenance.", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}