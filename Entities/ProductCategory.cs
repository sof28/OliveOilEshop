namespace ModularCommerce.Entities;

public class ProductCategory
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? ParentCategoryId { get; set; }
    public ProductCategory? ParentCategory { get; set; }
    public ICollection<ProductCategory> ChildCategories { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}

