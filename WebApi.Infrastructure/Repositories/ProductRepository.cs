using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repositories;

/// <summary>
/// Product repository implementation
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context, ILogger<Repository<Product>> logger)
        : base(context, logger)
    {
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting product with ID: {Id} including category", id);
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        _logger.LogInformation("Getting all products including categories");
        return await _dbSet
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        _logger.LogInformation("Getting products by category ID: {CategoryId}", categoryId);
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        _logger.LogInformation("Getting all active products");
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        _logger.LogInformation("Getting products in price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();
    }
}
