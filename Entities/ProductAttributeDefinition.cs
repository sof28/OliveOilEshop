namespace ModularCommerce.Entities;

public class ProductAttributeDefinition
{
    public long Id { get; set; }
    public long ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }
    public required string Name { get; set; }
    public string Key { get; set; } = string.Empty;
    public bool IsVariantAxis { get; set; }
    public bool IsRequired { get; set; }
    public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = [];
}

