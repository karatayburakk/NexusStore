using NexusStore.API.Dtos;
using NexusStore.API.Entities;

namespace NexusStore.API.Repositories
{
  public interface ICategoryRepository
  {
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> CreateCategoryAsync(Category category);
    Task<Category?> UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
  }
}
