using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repositories;

/// <summary>
/// Category repository implementation
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context, ILogger<Repository<Category>> logger)
        : base(context, logger)
    {
    }

    public async Task<Category?> GetCategoryWithProductsAsync(int id)
    {
        _logger.LogInformation("Getting category with ID: {Id} including products", id);
        return await _dbSet
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync()
    {
        _logger.LogInformation("Getting all categories with product count");
        return await _dbSet
            .Include(c => c.Products)
            .ToListAsync();
    }
}
