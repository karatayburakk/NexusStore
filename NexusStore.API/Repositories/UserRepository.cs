using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;
using NexusStore.API.Entities;

namespace NexusStore.API.Repositories
{
    public class UserRepository(NexusDbContext context) : IUserRepository
    {
        private readonly NexusDbContext _context = context;

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            // Attach the user entity to the context
            _context.Users.Attach(user);

            // Mark the entity as modified
            _context.Entry(user).State = EntityState.Modified;

            // Explicitly mark RowVersion as modified to ensure concurrency check
            _context.Entry(user).Property(u => u.RowVersion).IsModified = true;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}