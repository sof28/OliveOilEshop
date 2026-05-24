namespace OliveOilEshop.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
