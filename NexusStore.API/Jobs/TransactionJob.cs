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
    public async Task<bool> ProcessTransaction()
    {
      using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

      try
      {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);

        if (entity != null)
        {

          // Simulate delay to mimic row being changed before updating
          await Task.Delay(30000);

          entity.Username = "new_username6";
          await _context.SaveChangesAsync();

        }

        var product = await _context.Products
           .Where(p => p.Id == 1)
           .FirstOrDefaultAsync();


        // This line will throw an exception because the product does not exist
        // And rollback will be activated
        // product.Price = 100;
        // await _context.SaveChangesAsync();

        if (product != null)
        {
          product.Price = 100;
          await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();
        return true;

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