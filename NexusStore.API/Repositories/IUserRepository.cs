
using NexusStore.API.Entities;

namespace NexusStore.API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string email);
    }
}