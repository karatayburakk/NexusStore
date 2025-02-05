using Hangfire;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using NexusStore.API.Services;
using Microsoft.Extensions.Logging;

namespace NexusStore.API.Jobs
{
  public class JobScheduler
  {
    private static readonly ILogger<JobScheduler> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<JobScheduler>();

    public static void ScheduleJobs()
    {
      RecurringJob.AddOrUpdate<IEmailService>(
          "send-daily-email",
          emailService => emailService.SendDailyEmailAsync(),
          "0 9 * * *",
          new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

      RecurringJob.AddOrUpdate<TransactionJob>(
          "process-transaction",
          transactionJob => transactionJob.ProcessTransaction(),
          "0 9 * * *",
          new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

      GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });
      GlobalJobFilters.Filters.Add(new JobFailedFilter());
    }

    private class JobFailedFilter : JobFilterAttribute, IServerFilter
    {
      public void OnPerformed(PerformedContext filterContext)
      {
        if (filterContext.Exception != null)
        {
          SendFailureNotification(filterContext);
        }
      }

      public void OnPerforming(PerformingContext filterContext)
      {
        // No implementation needed
      }

      private static void SendFailureNotification(PerformedContext context)
      {
        _logger.LogError(context.Exception, "Job {JobId} failed with exception: {ExceptionMessage}", context.BackgroundJob.Id, context.Exception.Message);

        // Implement email sending logic here
        // For example: emailService.SendFailureNotificationAsync(context.Exception);
      }
    }
  }
}
