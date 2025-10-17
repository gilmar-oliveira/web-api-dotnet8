using WebApi.Domain.Entities;

namespace WebApi.Domain.Interfaces;

/// <summary>
/// Category-specific repository interface
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetCategoryWithProductsAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync();
}
