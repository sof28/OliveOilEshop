namespace ModularCommerce.Entities;

public class ProductType
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ProductAttributeDefinition> AttributeDefinitions { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}

