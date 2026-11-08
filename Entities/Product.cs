namespace ModularCommerce.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Sku { get; set; }
        public int StockQuantity { get; set; }
        public long? CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
        public long? ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }
        public ICollection<ProductVariant> Variants { get; set; } = [];
        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = [];

        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}

