using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;
using NexusStore.API.Entities;

namespace NexusStore.API.Repositories
{
  public class CategoryRepository(NexusDbContext context) : ICategoryRepository
  {
    private readonly NexusDbContext _context = context;

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
      return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
      return await _context.Categories.FindAsync(id);
    }

    public async Task<Category?> CreateCategoryAsync(Category category)
    {
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();
      return category;
    }

    public async Task<Category?> UpdateCategoryAsync(Category category)
    {
      _context.Categories.Attach(category);
      await _context.SaveChangesAsync();
      return category;
    }

    public async Task DeleteCategoryAsync(Category category)
    {
      _context.Categories.Remove(category);
      await _context.SaveChangesAsync();
    }
  }
}
