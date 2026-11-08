namespace ModularCommerce.Entities;

public class Coupon
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public bool FreeShipping { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal MinOrderAmount { get; set; }
    public DateTime? ValidUntil { get; set; }
    public bool IsActive { get; set; } = true;
}
