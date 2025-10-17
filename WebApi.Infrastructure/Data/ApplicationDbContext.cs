using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Data;

/// <summary>
/// Application database context supporting multiple database providers
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Price)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Configure relationship
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for performance
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.IsActive);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var categories = new[]
        {
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                Description = "Books and publications",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = 3,
                Name = "Clothing",
                Description = "Apparel and fashion items",
                CreatedAt = DateTime.UtcNow
            }
        };

        var products = new[]
        {
            new Product
            {
                Id = 1,
                Name = "Smartphone XYZ",
                Description = "Latest smartphone with advanced features",
                Price = 999.99m,
                Stock = 50,
                IsActive = true,
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "Laptop Pro",
                Description = "High-performance laptop for professionals",
                Price = 1499.99m,
                Stock = 30,
                IsActive = true,
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "Programming C#",
                Description = "Comprehensive guide to C# programming",
                Price = 49.99m,
                Stock = 100,
                IsActive = true,
                CategoryId = 2,
                CreatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);
    }
}
