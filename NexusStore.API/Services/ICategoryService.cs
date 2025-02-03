using NexusStore.API.Dtos;

namespace NexusStore.API.Services
{
  public interface ICategoryService
  {
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
    Task DeleteCategoryAsync(int id);
  }
}
