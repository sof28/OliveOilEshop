namespace ModularCommerce.Entities;

public class ProductVariant
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public required string Sku { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsDefault { get; set; }
    public ICollection<ProductAttributeValue> AttributeValues { get; set; } = [];
}

