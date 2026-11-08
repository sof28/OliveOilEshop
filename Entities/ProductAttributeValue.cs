namespace ModularCommerce.Entities;

public class ProductAttributeValue
{
    public long Id { get; set; }
    public long ProductAttributeDefinitionId { get; set; }
    public ProductAttributeDefinition? ProductAttributeDefinition { get; set; }
    public long? ProductId { get; set; }
    public Product? Product { get; set; }
    public long? ProductVariantId { get; set; }
    public ProductVariant? ProductVariant { get; set; }
    public required string Value { get; set; }
}

