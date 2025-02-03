using System.Data;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using NexusStore.API.Data;

namespace NexusStore.API.Jobs
{
  public class TransactionJob(NexusDbContext context, ILogger<TransactionJob> logger)
  {
    private readonly NexusDbContext _context = context;
    private readonly ILogger<TransactionJob> _logger = logger;

    // [DisableConcurrentExecution(timeoutInSeconds: 300)]
    public bool ProcessTransaction()
    {
      using var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable);

      try
      {
        var entity = _context.Users.FirstOrDefault(u => u.Id == 1);

        if (entity != null)
        {

          // Simulate delay to mimic row being changed before updating
          Task.Delay(30000).Wait();

          entity.Username = "new_username6";
          _context.SaveChanges();

          transaction.Commit();
          return true; // Indicate success
        }
        return false; // Entity not found
      }
      catch (DbUpdateConcurrencyException ex)
      {
        _logger.LogWarning(ex, "Concurrency conflict occurred.");
        transaction.Rollback();
        return false; // Indicate failure
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred during ProcessTransaction.");
        transaction.Rollback();
        return false; // Indicate failure
      }
    }
  }
}