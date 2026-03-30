using OliveOilEshop.Entities;

namespace OliveOilEshop.Features.OrderFeature.UpdateOrder
{
    public sealed record UpdateOrderResponse(long Id, DateTime OrderDate, decimal TotalPrice, string CustomerAddress, string CustomerEmail, string CustomerName, List<OrderItem> OrderItems, long? UserId);
}