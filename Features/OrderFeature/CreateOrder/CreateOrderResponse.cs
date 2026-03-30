using OliveOilEshop.Entities;

namespace OliveOilEshop.Features.OrderFeature.CreateOrder
{
    public sealed record CreateOrderResponse(DateTime OrderDate, decimal TotalPrice, string CustomerAddress, string CustomerEmail, string CustomerName, List<OrderItem> OrderItems, long? UserId);
}