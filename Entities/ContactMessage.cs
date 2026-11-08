namespace ModularCommerce.Entities;

public class ContactMessage
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string Message { get; set; }
    public bool ConsentGiven { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
