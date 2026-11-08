using ModularCommerce.Entities;

namespace ModularCommerce.Features.OrderFeature.UpdateOrder
{
    public sealed record UpdateOrderRequest(long Id, decimal? Price, string? CustomerAddress, string? CustomerName, string? CustomerEmail, List<OrderItem> OrderItems, long? UserId);
}
