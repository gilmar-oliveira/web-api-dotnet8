using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces;
using WebApi.DTOs;

namespace WebApi.Controllers;

/// <summary>
/// RESTful API controller for managing categories
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryRepository categoryRepository, ILogger<CategoriesController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>List of all categories</returns>
    /// <response code="200">Returns the list of categories</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
    {
        _logger.LogInformation("GET api/categories - Fetching all categories");
        
        var categories = await _categoryRepository.GetCategoriesWithProductCountAsync();
        var categoryDtos = categories.Select(c => MapToDto(c));
        
        _logger.LogInformation("Successfully retrieved {Count} categories", categoryDtos.Count());
        return Ok(categoryDtos);
    }

    /// <summary>
    /// Get a specific category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Category details</returns>
    /// <response code="200">Returns the category</response>
    /// <response code="404">If the category is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        _logger.LogInformation("GET api/categories/{Id} - Fetching category", id);
        
        var category = await _categoryRepository.GetCategoryWithProductsAsync(id);
        
        if (category == null)
        {
            _logger.LogWarning("Category with ID {Id} not found", id);
            return NotFound(new { message = $"Category with ID {id} not found" });
        }
        
        _logger.LogInformation("Successfully retrieved category with ID {Id}", id);
        return Ok(MapToDto(category));
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="createCategoryDto">Category data</param>
    /// <returns>Created category</returns>
    /// <response code="201">Returns the newly created category</response>
    /// <response code="400">If the category data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        _logger.LogInformation("POST api/categories - Creating new category: {CategoryName}", createCategoryDto.Name);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for category creation");
            return BadRequest(ModelState);
        }

        var category = new Category
        {
            Name = createCategoryDto.Name,
            Description = createCategoryDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        var createdCategory = await _categoryRepository.GetCategoryWithProductsAsync(category.Id);
        
        _logger.LogInformation("Successfully created category with ID {Id}", category.Id);
        
        return CreatedAtAction(
            nameof(GetCategory), 
            new { id = category.Id }, 
            MapToDto(createdCategory!));
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="updateCategoryDto">Updated category data</param>
    /// <returns>No content</returns>
    /// <response code="204">If the category was updated successfully</response>
    /// <response code="400">If the category data is invalid</response>
    /// <response code="404">If the category is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
    {
        _logger.LogInformation("PUT api/categories/{Id} - Updating category", id);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for category update");
            return BadRequest(ModelState);
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        
        if (category == null)
        {
            _logger.LogWarning("Category with ID {Id} not found for update", id);
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        category.Name = updateCategoryDto.Name;
        category.Description = updateCategoryDto.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category);
        await _categoryRepository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully updated category with ID {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the category was deleted successfully</response>
    /// <response code="404">If the category is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation("DELETE api/categories/{Id} - Deleting category", id);
        
        var exists = await _categoryRepository.ExistsAsync(id);
        
        if (!exists)
        {
            _logger.LogWarning("Category with ID {Id} not found for deletion", id);
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        await _categoryRepository.DeleteAsync(id);
        await _categoryRepository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully deleted category with ID {Id}", id);
        return NoContent();
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = category.Products?.Count ?? 0,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}
