namespace WebApi.Domain.Entities;

/// <summary>
/// Product entity representing a product in the system
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation property
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
