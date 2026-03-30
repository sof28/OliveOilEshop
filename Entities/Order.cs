using OliveOilEshop.Enums;

namespace OliveOilEshop.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerEmail { get; set; }
    public required string CustomerAddress { get; set; }
    public required OrderStatus Status { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];

    // Optional: Link to authenticated user (null for guest orders)
    public long? UserId { get; set; }
    public User? User { get; set; }
}