using Hangfire;
using NexusStore.API.Services;

namespace NexusStore.API.Jobs
{
  public class JobScheduler
  {
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
    }
  }
}
