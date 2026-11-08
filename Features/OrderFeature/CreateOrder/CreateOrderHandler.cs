using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Enums;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.OrderFeature.CreateOrder
{
    public sealed class CreateOrderHandler(IRepository<Order> orderRepository, IUnitOfWork unitOfWork) : IHandler<CreateOrderRequest, Result<CreateOrderResponse>>
    {
        public async Task<Result<CreateOrderResponse>> HandleAsync(CreateOrderRequest command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalPrice = command.Price,
                CustomerAddress = command.CustomerAddress,
                CustomerName = command.CustomerName,
                CustomerEmail = command.CustomerEmail,
                OrderItems = [.. command.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                })],
                UserId = command.UserId,
                Status = OrderStatus.InProgress
            };
            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(new CreateOrderResponse(
              order.OrderDate,
              order.TotalPrice,
              order.CustomerAddress,
              order.CustomerEmail,
              order.CustomerName,
              [.. order.OrderItems.Select(oi => new OrderItem
              {
                  ProductId = oi.ProductId,
                  Quantity = oi.Quantity,
                  UnitPrice = oi.UnitPrice
              })],
              order.UserId
            ));
        }
    }
}
