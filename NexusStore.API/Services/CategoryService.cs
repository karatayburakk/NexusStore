using NexusStore.API.Entities;
using NexusStore.API.Repositories;
using AutoMapper;
using NexusStore.API.Dtos;

namespace NexusStore.API.Services
{
  public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
    {
      var categories = await _categoryRepository.GetAllCategoriesAsync();
      return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
    }

    public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
    {
      var category = await _categoryRepository.GetCategoryByIdAsync(id) ?? throw new KeyNotFoundException("Category not found.");
      return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
      var category = _mapper.Map<Category>(createCategoryDto);
      var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
      return _mapper.Map<CategoryResponseDto>(createdCategory);
    }

    public async Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
      var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id) ?? throw new KeyNotFoundException("Category not found.");

      _mapper.Map(updateCategoryDto, existingCategory);

      var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
      return _mapper.Map<CategoryResponseDto>(updatedCategory);
    }

    public async Task DeleteCategoryAsync(int id)
    {
      var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id) ?? throw new KeyNotFoundException("Category not found.");

      await _categoryRepository.DeleteCategoryAsync(existingCategory);
    }
  }
}
