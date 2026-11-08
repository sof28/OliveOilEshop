using ModularCommerce.Entities;

namespace ModularCommerce.Features.OrderFeature.CreateOrder
{
    public sealed record CreateOrderRequest(decimal Price, string CustomerAddress, string CustomerName, string CustomerEmail, List<OrderItem> OrderItems, long? UserId);
}
