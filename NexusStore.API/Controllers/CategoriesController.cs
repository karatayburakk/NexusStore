using Microsoft.AspNetCore.Mvc;
using NexusStore.API.Services;
using NexusStore.API.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace NexusStore.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/[controller]")]
  public class CategoriesController(ICategoryService categoryService) : ControllerBase
  {
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var categories = await _categoryService.GetAllCategoriesAsync();
      return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var category = await _categoryService.GetCategoryByIdAsync(id);
      return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
    {
      var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);
      return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto updateCategoryDto)
    {
      var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
      return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var categoryExists = await _categoryService.GetCategoryByIdAsync(id) != null;
      if (!categoryExists) return NotFound();
      await _categoryService.DeleteCategoryAsync(id);
      return NoContent();
    }
  }
}
