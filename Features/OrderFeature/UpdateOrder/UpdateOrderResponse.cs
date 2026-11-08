using ModularCommerce.Entities;

namespace ModularCommerce.Features.OrderFeature.UpdateOrder
{
    public sealed record UpdateOrderResponse(long Id, DateTime OrderDate, decimal TotalPrice, string CustomerAddress, string CustomerEmail, string CustomerName, List<OrderItem> OrderItems, long? UserId);
}
