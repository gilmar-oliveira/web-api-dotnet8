namespace WebApi.Domain.Entities;

/// <summary>
/// Category entity for organizing products
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
