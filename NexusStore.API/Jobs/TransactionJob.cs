using System.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;

// Version check off all tables before updating
// Transaction Roolback: Transaction fail because product does not exists, so transaction will be rolled back
// DisableConcurrentExecution attribute added to avoid concurrency conflicts
namespace NexusStore.API.Jobs
{
  public class TransactionJob(NexusDbContext context, ILogger<TransactionJob> logger)
  {
    private readonly NexusDbContext _context = context;
    private readonly ILogger<TransactionJob> _logger = logger;

    [DisableConcurrentExecution(timeoutInSeconds: 300)]
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

        var product = _context.Products
           .Where(p => p.Id == 1)
           .FirstOrDefault();

        if (product != null)
        {
          product.Price = 100;
          _context.SaveChanges();
        }

        return false; // Entity not found
      }
      catch (DbUpdateConcurrencyException ex)
      {
        _logger.LogWarning(ex, "Concurrency conflict occurred.");
        transaction.Rollback();
        throw; // Rethrow the exception to mark the job as failed
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred during ProcessTransaction.");
        transaction.Rollback();
        throw; // Rethrow the exception to mark the job as failed
      }
    }
  }
}