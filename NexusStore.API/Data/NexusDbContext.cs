using Microsoft.EntityFrameworkCore;
using NexusStore.API.Entities;

namespace NexusStore.API.Data
{
    public class NexusDbContext(DbContextOptions<NexusDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Category>().ToTable("categories");

            // Configure RowVersion for entities that implement IAuditableEntity.
        }

        public override int SaveChanges()
        {
            SetModifiedStateForUpdatedEntities();
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetModifiedStateForUpdatedEntities();
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetModifiedStateForUpdatedEntities()
        {
            // Get entries that are being updated.
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity && e.State == EntityState.Detached);

            foreach (var entry in entries)
            {
                // Set the state to Modified.
                Console.WriteLine($"Setting state to Modified for entity {entry.Entity.GetType().Name}");
                entry.State = EntityState.Modified;
            }
        }

        private void UpdateAuditFields()
        {
            // Get entries that are either newly added or modified and implement IAuditableEntity.
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                // On adding, set both CreatedAt and UpdatedAt to the current time.
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                // Always update UpdatedAt.
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

    }
}