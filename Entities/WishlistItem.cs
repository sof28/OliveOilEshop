namespace ModularCommerce.Entities;

public class WishlistItem
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
