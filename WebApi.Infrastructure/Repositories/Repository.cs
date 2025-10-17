using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebApi.Domain.Interfaces;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation with common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<Repository<T>> _logger;

    public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _logger = logger;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting entity {EntityType} with ID: {Id}", typeof(T).Name, id);
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogInformation("Getting all entities of type {EntityType}", typeof(T).Name);
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        _logger.LogInformation("Finding entities of type {EntityType} with predicate", typeof(T).Name);
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        _logger.LogInformation("Adding new entity of type {EntityType}", typeof(T).Name);
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _logger.LogInformation("Updating entity of type {EntityType}", typeof(T).Name);
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting entity {EntityType} with ID: {Id}", typeof(T).Name, id);
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        _logger.LogInformation("Checking if entity {EntityType} with ID {Id} exists", typeof(T).Name, id);
        var entity = await _dbSet.FindAsync(id);
        return entity != null;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        _logger.LogInformation("Saving changes to database");
        return await _context.SaveChangesAsync();
    }
}
