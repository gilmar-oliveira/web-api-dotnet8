using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces;
using WebApi.DTOs;

namespace WebApi.Controllers;

/// <summary>
/// RESTful API controller for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        _logger.LogInformation("GET api/products - Fetching all products");
        
        var products = await _productRepository.GetAllAsync();
        var productDtos = products.Select(p => MapToDto(p));
        
        _logger.LogInformation("Successfully retrieved {Count} products", productDtos.Count());
        return Ok(productDtos);
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="404">If the product is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        _logger.LogInformation("GET api/products/{Id} - Fetching product", id);
        
        var product = await _productRepository.GetByIdAsync(id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        _logger.LogInformation("Successfully retrieved product with ID {Id}", id);
        return Ok(MapToDto(product));
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of products in the category</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        _logger.LogInformation("GET api/products/category/{CategoryId} - Fetching products by category", categoryId);
        
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
        var productDtos = products.Select(p => MapToDto(p));
        
        _logger.LogInformation("Successfully retrieved {Count} products for category {CategoryId}", 
            productDtos.Count(), categoryId);
        
        return Ok(productDtos);
    }

    /// <summary>
    /// Get active products
    /// </summary>
    /// <returns>List of active products</returns>
    /// <response code="200">Returns the list of active products</response>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProducts()
    {
        _logger.LogInformation("GET api/products/active - Fetching active products");
        
        var products = await _productRepository.GetActiveProductsAsync();
        var productDtos = products.Select(p => MapToDto(p));
        
        _logger.LogInformation("Successfully retrieved {Count} active products", productDtos.Count());
        return Ok(productDtos);
    }

    /// <summary>
    /// Get products by price range
    /// </summary>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <returns>List of products in the price range</returns>
    /// <response code="200">Returns the list of products</response>
    /// <response code="400">If the price range is invalid</response>
    [HttpGet("price-range")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPriceRange(
        [FromQuery] decimal minPrice, 
        [FromQuery] decimal maxPrice)
    {
        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
        {
            _logger.LogWarning("Invalid price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
            return BadRequest(new { message = "Invalid price range" });
        }

        _logger.LogInformation("GET api/products/price-range - Fetching products in range {MinPrice} - {MaxPrice}", 
            minPrice, maxPrice);
        
        var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
        var productDtos = products.Select(p => MapToDto(p));
        
        _logger.LogInformation("Successfully retrieved {Count} products in price range", productDtos.Count());
        return Ok(productDtos);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="createProductDto">Product data</param>
    /// <returns>Created product</returns>
    /// <response code="201">Returns the newly created product</response>
    /// <response code="400">If the product data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        _logger.LogInformation("POST api/products - Creating new product: {ProductName}", createProductDto.Name);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for product creation");
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock,
            IsActive = createProductDto.IsActive,
            CategoryId = createProductDto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        var createdProduct = await _productRepository.GetByIdAsync(product.Id);
        
        _logger.LogInformation("Successfully created product with ID {Id}", product.Id);
        
        return CreatedAtAction(
            nameof(GetProduct), 
            new { id = product.Id }, 
            MapToDto(createdProduct!));
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="updateProductDto">Updated product data</param>
    /// <returns>No content</returns>
    /// <response code="204">If the product was updated successfully</response>
    /// <response code="400">If the product data is invalid</response>
    /// <response code="404">If the product is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("PUT api/products/{Id} - Updating product", id);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for product update");
            return BadRequest(ModelState);
        }

        var product = await _productRepository.GetByIdAsync(id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found for update", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;
        product.Stock = updateProductDto.Stock;
        product.IsActive = updateProductDto.IsActive;
        product.CategoryId = updateProductDto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully updated product with ID {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the product was deleted successfully</response>
    /// <response code="404">If the product is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("DELETE api/products/{Id} - Deleting product", id);
        
        var exists = await _productRepository.ExistsAsync(id);
        
        if (!exists)
        {
            _logger.LogWarning("Product with ID {Id} not found for deletion", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        await _productRepository.DeleteAsync(id);
        await _productRepository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully deleted product with ID {Id}", id);
        return NoContent();
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
